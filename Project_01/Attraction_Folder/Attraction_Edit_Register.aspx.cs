using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Project_01
{
    public partial class Attraction_Edit_Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //שהחזירה עצמי דרופדאוןליסט FillArrayListForDropDownList שקיבלנו מהפעלת הפעולה  ArrayList הכנסת ערכי אטרקציה (מזהה ושם אטרקציה) לדרופדאוןליסט מתוך  
                ArrayList arr = Connect.FillArrayListForDropDownList("SELECT AttractionType_Type,AttractionType_ID FROM AttractionType; ",
                    "AttractionType_Type", "AttractionType_ID");
                for (int i = 0; i < arr.Count; i++) 
                {
                    Attraction_Type.Items.Add((ListItem)arr[i]);
                }
                //Attraction_Type.Items.Add(new ListItem("ok","1"));// ok = הטקסט המוצג 
                //Attraction_Type.Items.Add(new ListItem("hi", "2"));// המשך - 1 = הערך , אם אין ערך הטקסט המוצג הוא הערך כבררית מחדל

                // שמירת פרטי אטרקציה בעצם מסוג אטרקציה הנשמר בסשן
                Session["Attraction"] = AttractionService.FillAttraction("SELECT * FROM Attraction INNER JOIN AttractionType ON " +
                    "Attraction.Attraction_Type = AttractionType.AttractionType_ID WHERE Attraction_ID = 28 ");
                Attraction a = (Attraction)Session["Attraction"];//  TextBox השמת ערכים קיימים של אטרקציה ב
                Attraction_Name.Text = a.Attraction_Name;//
                Attraction_Type.SelectedValue = a.Attraction_TypeID;//לבדוק אם עובד
                Attraction_MinAge.Text = a.Attraction_MinAge;//
                Attraction_MaxAge.Text = a.Attraction_MaxAge;//
                Attraction_Price.Text = a.Attraction_Price;//
                Attraction_Duration.Text = a.Attraction_Duration;//
                Attraction_Address.Text = a.Attraction_Address;//
                Attraction_Gmail.Text = a.Attraction_Gmail;//
                Attraction_PhonNumber.Text = a.Attraction_PhonNumber;//
                Attraction_RecommendedMonth.SelectedValue = a.Attraction_recommendedMonth;//לבדוק אם עובד
                Attraction_FreeEntry.SelectedValue = a.Attraction_FreeEntry;//לבדוק אם עובד
                Attraction_Text.Text = a.Attraction_Text;//
                Session["filelocation"] = a.Attraction_Photo;//
            }
        }

        // UpdateAtt עדכון פרטי אטרקציה והצגת טקסט מתאים - משתמש בפעולה 
        protected async void Button1_Click(object sender, EventArgs e) 
        {
            // יוצר נתיב תמונה במידה וקיימת ,שומר אותו בסשן כמחרוזת, שומר את התמונה בתקיית תמונות השמורה בפרוייקט
            if (FileUpload1.PostedFile.FileName == "")
            {
                Label16.Text = "No file name";
            }
            else
            {
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                FileUpload1.PostedFile.SaveAs(Server.MapPath("~\\pictures\\" + filename)); // שמירה בתיקיה
                string filelocation = "~\\pictures\\" + filename; // השדה שישלח לשמירה בבסיס הנתונים
                Session["filelocation"] = filelocation;
            }
            // עדכון פרטים
            Result.Text =await AttractionService.UpdateAtt(28, Attraction_Name.Text, Attraction_Type.SelectedValue, Attraction_MinAge.Text,
                Attraction_MaxAge.Text, Attraction_Price.Text, Attraction_Duration.Text,
                Attraction_Address.Text, Attraction_Gmail.Text, Attraction_PhonNumber.Text, Attraction_RecommendedMonth.SelectedValue,
                Attraction_FreeEntry.SelectedValue, Attraction_Text.Text, Session["filelocation"].ToString());

            // אם הכתובת עודכה מעדכן מסלול כל האטרקציות
            if(Attraction_Address.Text != ((Attraction)Session["Attraction"]).Attraction_Address)
            {
                List<Attraction> attractions = AttractionService.GetAttractionsFromDataBase();
                attractions = AttractionService.FindEfficientPath(attractions);
                AttractionService.UpdatePathToDataBase(attractions);
            }
        }

    }
}