using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections;

namespace Project_01
{
    public partial class Attraction_Owner : System.Web.UI.Page
    {
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (((User)Session["User"]).User_Type == "אדמין")//אם נכנס כאדמין
                {
                    User_Edit.Visible = false;
                    AddAttraction.Visible = false;
                }

                    if (((User)Session["User"]).User_Type != "אדמין")//בעל עסק
                {
                    //מאסטר פייג
                    Site master = (Site)this.Master;
                    master.MasterPageSignUpOut.Text = "התנתק";
                    master.MasterPageOrders.Visible = false;
                    master.MasterPageSignUpOut.CommandName = "/HomePage"; //התנתקות
                    master.MasterPageNewOrder.CommandName = "/Attraction_Folder/Attractions_Display";
                    master.MasterPageAbout.CommandName = "/About";
                    master.MasterPageLogo.CommandName = "/Attraction_Folder/Attraction_Owner";
                }
                else//אדמין
                {
                    //מאסטר פייג
                    Site master = (Site)this.Master;
                    master.MasterPageSignUpOut.Text = "התנתק";
                    master.MasterPageOrders.Visible = false;
                    master.MasterPageSignUpOut.CommandName = "/Homepage"; //התנתקות
                    master.MasterPageNewOrder.CommandName = "/Attraction_Folder/AttractionDisplay_Admin";
                    master.MasterPageAbout.CommandName = "/About";
                    master.MasterPageLogo.CommandName = "/admin";
                }

                    //אם נכנס לעמוד כאדמין שצופה במשתמש או כמשתמש
                string User_Name; 
                if (Session["UserOfWatch"] != null)
                    User_Name = Session["UserOfWatch"].ToString();
                else
                    User_Name = ((User)Session["User"]).User_Name;

                //דאטאסט 
                if (Session["Attraction_AttOwner"] == null)
                {
                    //דאטאסט ימים אטרקציות של משתמש
                    ds = Connect.Connect_DataSet("SELECT * FROM Attraction WHERE Attraction_Valid = true AND User_Name = '" + User_Name + "'", "Attraction");
                    Session["Attraction_AttOwner"] = ds;
                }
                else
                    ds = (DataSet)Session["Attraction_AttOwner"];

                Attractions.DataSource = ds;
                Attractions.DataBind();
            }
        }


        //העברה לעמוד עריכת אטרקציה + מחיקת אטרקציה
        protected void Attractions_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Session["Attraction_ID_EditAtt"] = Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text);
                //Move to another page 
                Response.Redirect("Attraction_Register_Edit.aspx"); // העברה לדף עריכת מסלול
            }

            if (e.CommandName == "remove")
            {
                AttractionService.RemoveAttraction(Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text));

                //עדכון התצוגה
                Session["Attraction_AttOwner"] = null; //כדי שיעדכן את התצוגה בפעם הבאה שיחזור לדף בעל עסק null  השמת 
                // Move to another page 
                Response.Redirect("Attraction_Owner.aspx"); // העברה לדף בעל עסק
            }
        }


        //אטרקציות - דאטאבאונד
        protected void Attractions_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            // Check if the Repeater item is an item or alternating item (not a header/footer)
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                foreach (DataRow row in ((DataSet)Session["Attraction_AttOwner"]).Tables["Attraction"].Rows)
                {
                    if ((int)row["Attraction_ID"] == Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text))
                    {
                        //המין ששיבץ הכי הרבה את האטרקציה
                        object MainGender = ((Connect.Connect_ExecuteScalar("SELECT TOP 1 Users.[User_Gender] FROM(([Orders] INNER JOIN[Users]" +
                            " ON[Orders].[Order_UserName] = [Users].[User_Name]) INNER JOIN[Days] ON[Orders].[Order_ID] = [Days].[Order_ID])" +
                            " INNER JOIN[Day_Attraction] ON[Days].[Day_ID] = [Day_Attraction].[Day_ID] WHERE[Day_Attraction].[Attraction_Id] = "
                            + ((Label)e.Item.FindControl("Attraction_ID")).Text + " GROUP BY Users.[User_Gender] ORDER BY COUNT(*) DESC")));
                        if (MainGender != null)
                            ((Label)e.Item.FindControl("MainGender")).Text = MainGender.ToString();
                        else
                            ((Label)e.Item.FindControl("MainGender")).Text = "אין";// אם לא שובץ בכלל באטרקציה 

                        //מספר שיבוצים במסלולים
                        ((Label)e.Item.FindControl("AddNumber")).Text = Connect.Connect_ExecuteScalar("SELECT COUNT(*) AS TotalRows FROM Day_Attraction WHERE Attraction_ID = " + ((Label)e.Item.FindControl("Attraction_ID")).Text).ToString();

                    }
                }
            }
        }

        //הוספת אטרקציה
        protected void AddAttraction_Click(object sender, EventArgs e)
        {
            Session["Attraction_ID_EditAtt"] = null; // לוודא שלא בטעות יתן עדכון פרטי אטרקציה במקום הרשמה
            //עדכון התצוגה
            Session["Attraction_AttOwner"] = null; //כדי שיעדכן את התצוגה בפעם הבאה שיחזור לדף בעל עסק null  השמת 
            // Move to another page 
            Response.Redirect("Attraction_Register_Edit.aspx"); // העברה לדף בעל עסק
        }

        //העברה לעריכת פרטי אטרקציה
        protected void User_Edit_Click(object sender, EventArgs e)
        {
            Response.Redirect("/User_Folder/User_Edit.aspx"); // העברה לדף עריכת פרטים
        }
    }
}