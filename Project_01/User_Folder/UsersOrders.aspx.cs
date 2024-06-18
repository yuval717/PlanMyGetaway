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
    public partial class UsersOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = null; //יצירת  דאטאסט ריק לצרכים שונים
            string User_Name;
            if (Session["UserOfWatch"] != null)
                User_Name = Session["UserOfWatch"].ToString();
            else
                User_Name = ((User)Session["User"]).User_Name;

            if (!Page.IsPostBack)
            {
                MasterPage_UserName.Text = ((User)Session["User"]).User_Name;
                if (((User)Session["User"]).User_Type != "אדמין")//משתמש רשום
                {
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

                Session["UserOrders_OrderDis"] = null;
                //*** ההזמנות האחרונות של משתמש מחובר - ממויין מחדש לישן
                if (Session["UserOrders_OrderDis"] == null)
                {
                       ds = Connect.Connect_DataSet("SELECT  * FROM Orders WHERE  NotValid = " + false + " AND Order_UserName = '"
                        + User_Name + "'  ORDER BY Order_AddDate DESC", "Orders");
                    Session["UserOrders_OrderDis"] = ds;
                }
                else
                    ds = (DataSet)Session["UserOrders_OrderDis"];
                DataList2.DataSource = ds;
                DataList2.DataBind();
                NoResult_Lable.Visible = false; // איפוס לתצוגה
                if (((DataSet)DataList2.DataSource).Tables["Orders"].Rows.Count == 0) // אם לא נוצרו/נמחקו חופשות
                {
                    NoResult_Lable.Visible = true; // הודעת אין חופשות
                }
            }
        }

        //דאטא באונד לחופשות
        protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Label days = (Label)e.Item.FindControl("Label2");
            days.Text += " ימים";

             
            if ( DateTime.Parse((((Label)e.Item.FindControl("Datesituation")).Text).Substring(0, 10)) < DateTime.Now)
            {
                ((Label)e.Item.FindControl("Datesituation")).Text = "התקיימה";
            }
            else
                ((Label)e.Item.FindControl("Datesituation")).Text = "עתידה להתקיים";
        }


        //העברה לדף חופשה
        protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //Move to another page to show the product
            if (e.CommandName == "DoShow")
            {
                Session["OrderCode_ForPathDIs"] = ((Label)e.Item.FindControl("Order_ID")).Text;
                Session["from"] = "/User_Folder/UsersOrders.aspx";
                if (((Label)e.Item.FindControl("OrderType")).Text == "ידני") // מציאת סוג האטרקציה - לפיה יעבור לעמוד תצוגה
                    Response.Redirect("/Order_Folder/Manual_Order.aspx");
                else
                    Response.Redirect("/Order_Folder/AutomaticPathDisplay.aspx");
            }
        }

    }
}
