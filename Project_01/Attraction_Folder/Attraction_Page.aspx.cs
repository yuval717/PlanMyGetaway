using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.OleDb;


namespace Project_01
{
    public partial class AttractionPage : System.Web.UI.Page
    {
        public static ArrayList filterdAttractionsForAttractoinPage; // תוכן הארייליסט של סשן סינון אטרקציות - תצוגת אטרקציות
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if(Session["User"] ==null)//משתמש אורח
                {
                    //מאסטר פייג
                    Site master = (Site)this.Master;
                    master.MasterPageSignUpOut.Text = "התחבר";
                    master.MasterPageOrders.Visible = false;
                    master.MasterPageSignUpOut.CommandName = "/User_Folder/User_Login_Register";
                    master.MasterPageAbout.CommandName = "/About";
                    master.MasterPageLogo.CommandName = "/Homepage";
                    master.MasterPageNewOrder.CommandName = "/Attraction_Folder/Attractions_Display";
                }
                else if (((User)Session["User"]).User_Type == "בעל עסק")//בעל עסק
                {
                    MasterPage_UserName.Text = ((User)Session["User"]).User_Name;
                    //מאסטר פייג
                    Site master = (Site)this.Master;
                    master.MasterPageSignUpOut.Text = "התנתק";
                    master.MasterPageOrders.Visible = false;
                    master.MasterPageSignUpOut.CommandName = "/HomePage"; //התנתקות
                    master.MasterPageNewOrder.CommandName = "/Attraction_Folder/Attractions_Display";
                    master.MasterPageAbout.CommandName = "/About";
                    master.MasterPageLogo.CommandName = "/Attraction_Folder/Attraction_Owner";
                }
                else if(((User)Session["User"]).User_Type == "אדמין")//אדמין
                {
                    MasterPage_UserName.Text = ((User)Session["User"]).User_Name;
                    //מאסטר פייג
                    Site master = (Site)this.Master;
                    master.MasterPageSignUpOut.Text = "התנתק";
                    master.MasterPageOrders.Visible = false;
                    master.MasterPageSignUpOut.CommandName = "/Homepage"; //התנתקות
                    master.MasterPageNewOrder.CommandName = "/Attraction_Folder/AttractionDisplay_Admin";
                    master.MasterPageAbout.CommandName = "/About";
                    master.MasterPageLogo.CommandName = "/admin";
                }
                else//משתמש רשום
                {
                    MasterPage_UserName.Text = ((User)Session["User"]).User_Name;
                    //מאסטר פייג
                    Site master = (Site)this.Master;
                    master.MasterPageSignUpOut.Text = "התנתק";
                    master.MasterPageOrders.Visible = true;
                    master.MasterPageOrders.CommandName = "/User_Folder/UsersOrders";
                    master.MasterPageSignUpOut.CommandName = "/Homepage"; //התנתקות
                    master.MasterPageNewOrder.CommandName = "/Attraction_Folder/Attractions_Display";
                    master.MasterPageAbout.CommandName = "/About";
                    master.MasterPageLogo.CommandName = "/Homepage";
                }

                filterdAttractionsForAttractoinPage = null; // איפוס התכונה
                if (Session["filterdAttractionsForAttractoinPage"] != null) // אם סשן הסינון קיים 
                {
                    filterdAttractionsForAttractoinPage = (ArrayList)Session["filterdAttractionsForAttractoinPage"]; // אכלוס התכונה
                }
                Session["filterdAttractionsForAttractoinPage"] = null;//איפוס הסשן למקרה ובו המשתמש יצא מהעמוד בעזרת התפריט ולא כפתור החזרה לתצוגה ואז יהיו בעיות


                //לעדכן בפעולה בונה אטרקציה מחיר, משך, טלפון, אימייל
                Attraction a = AttractionService.FillAttraction("SELECT * FROM Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type = AttractionType.AttractionType_ID WHERE Attraction.Attraction_ID =" + Session["AttractionID_ForAttractionPage"].ToString());
                Attraction_Name.Text = a.Attraction_Name;
                Attraction_Type.Text = a.Attraction_TypeName;
                Attraction_Age.Text = a.Attraction_MaxAge + " - " + a.Attraction_MinAge;
                Attraction_Price.Text = a.Attraction_Price + " שח";
                Attraction_Duration.Text = a.Attraction_Duration + " דקות";
                Attraction_Address.Text = a.Attraction_Address;
                Attraction_Gmail.Text = a.Attraction_Gmail;
                Attraction_PhonNumber.Text = a.Attraction_PhonNumber;
                Attraction_Text.Text = a.Attraction_Text;
                Attraction_BigPhoto.ImageUrl = a.Attraction_Photo;

                DataTable MorePhotosOfAttraction = Connect.Connect_DataTable("SELECT * FROM Photos WHERE Attraction_ID = " + Session["AttractionID_ForAttractionPage"].ToString(), "Photos");
                DataRow newRow = MorePhotosOfAttraction.NewRow();
                newRow["Attraction_ID"] = Session["AttractionID_ForAttractionPage"].ToString();
                newRow["FileLocation"] = Attraction_BigPhoto.ImageUrl;
                MorePhotosOfAttraction.Rows.Add(newRow);
                MorePhotos.DataSource = MorePhotosOfAttraction;
                MorePhotos.DataBind();
            }
        }

        //protected void Back_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect(Session["from"].ToString());
        //}


        //שינוי תמונה גדולה על ידי לחיצה על תמונה קטנה
        protected void MorePhotos_ItemCommand(object source, DataListCommandEventArgs e)
        {
            Attraction_BigPhoto.ImageUrl = ((ImageButton)e.Item.FindControl("Photo")).ImageUrl;
        }

        //כפתור חזרה
        protected void BackTo_Click(object sender, EventArgs e)
        {
            if (filterdAttractionsForAttractoinPage != null)// אם קיים סשן סינון - לתצוגת סינון בדף תצוגת אטרקציות
            {
                Session["filterdAttractionsForAttractoinPage"] = filterdAttractionsForAttractoinPage; //אכלוס הסשן
            }
            string s = Session["from"].ToString();
            Response.Redirect(s);
        }
    }
}