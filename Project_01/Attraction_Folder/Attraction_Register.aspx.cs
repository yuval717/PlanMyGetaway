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
    public partial class Attraction_Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ArrayList arr = Connect.FillArrayListForDropDownList("SELECT AttractionType_Type,AttractionType_ID FROM AttractionType; ", "AttractionType_Type"
                    , "AttractionType_ID");
                for (int i = 0; i < arr.Count; i++) //שהחזירה עצמי דרופדאוןליסט FillArrayListForDropDownList שקיבלנו מהפעלת הפעולה  ArrayList הכנסת ערכי אטרקציה (מזהה ושם אטרקציה) לדרופדאוןליסט מתוך  
                {
                    Attraction_Type.Items.Add((ListItem)arr[i]);
                }
                //Attraction_Type.Items.Add(new ListItem("ok","1"));// ok = הטקסט המוצג 
                //Attraction_Type.Items.Add(new ListItem("hi", "2"));// המשך - 1 = הערך , אם אין ערך הטקסט המוצג הוא הערך כבררית מחדל
            }
        }

        // RegisterAtt הרשמת אטרקציה והצגת טקסט מתאים - משתמש בפעולה 
        protected async void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.PostedFile.FileName == "")
            {
                Label16.Text = "No file name";
            }
            else
            {
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName) + DateTime.Now.Ticks;
                FileUpload1.PostedFile.SaveAs(Server.MapPath("~\\pictures\\" + filename)); // שמירה בתיקיה
                string filelocation = "~\\pictures\\" + filename; // השדה שישלח לשמירה בבסיס הנתונים
                Session["filelocation"] = filelocation;
            }

            

            // הרשמת אטרקציה
            //Result.Text = await AttractionService.RegisterAttAsync(Attraction_Name.Text, Attraction_Type.SelectedValue, Attraction_MinAge.Text,
            //    Attraction_MaxAge.Text, Attraction_Price.Text, Attraction_Duration.Text, Attraction_Address.Text,
            //    Attraction_Gmail.Text, Attraction_PhonNumber.Text, Attraction_RecommendedMonth.SelectedValue, Attraction_FreeEntry.SelectedValue,
            //    Attraction_Text.Text, Session["filelocation"].ToString());
        }

        protected void Attraction_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Attraction_Type.SelectedValue == "5")
            {
                LabelOPH.Visible = false;
                IndoorAttraction_OpeningHour.Visible = false;
                LabelCLH.Visible = false;
                IndoorAttraction_ClosingHour.Visible = false;
                LabePR.Visible = false;
                Attraction_Price.Visible = false;
                LabeDU.Visible = false;
                Attraction_Duration.Visible = false;
                LabelFR.Visible = false;
                Attraction_FreeEntry.Visible = false;
                LabelPH.Visible = true;
                Attraction_PhonNumber.Visible = true;
                LabelGM.Visible = true;
                Attraction_Gmail.Visible = true;
                LabelKIN.Visible = false;
                NatureAttraction_KilometersNumber.Visible = false;
                LabelDI.Visible = false;
                NatureAttraction_Difficulty.Visible = false;
            }

            if ((int)Connect.Connect_ExecuteScalar("SELECT VacationType_ID FROM AttractionType Where AttractionType_ID = " + Attraction_Type.SelectedValue) == 2)
            {
                LabelOPH.Visible = false;
                IndoorAttraction_OpeningHour.Visible = false;
                LabelCLH.Visible = false;
                IndoorAttraction_ClosingHour.Visible = false;
                LabePR.Visible = false;
                Attraction_Price.Visible = false;
                LabeDU.Visible = false;
                Attraction_Duration.Visible = false;
                LabelFR.Visible = false;
                Attraction_FreeEntry.Visible = false;
                LabelPH.Visible = false;
                Attraction_PhonNumber.Visible = false;
                LabelGM.Visible = false;
                Attraction_Gmail.Visible = false;
                LabelKIN.Visible = false;
                NatureAttraction_KilometersNumber.Visible = false;
                LabelDI.Visible = false;
                NatureAttraction_Difficulty.Visible = false;
            }
        }
    }
}