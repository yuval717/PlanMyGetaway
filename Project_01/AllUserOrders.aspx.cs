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
    public partial class AllUserOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["UserOrders"];
            DataList1.DataSource = ds;
            DataList1.DataBind();
        }

        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Label days = (Label)e.Item.FindControl("Label2");
            days.Text += " ימים";
        }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //מעבר לעמוד הזמנה
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("SmartPage.aspx");
        }
    }
}