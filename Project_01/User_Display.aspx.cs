using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;


namespace Project_01
{
    public partial class User_Display : System.Web.UI.Page
    {
        private DataSet ds = null;
        private DataSet dsdefault = null; //ללא שום סינון  DataSet ה 
        private DataTable Block = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["Users"] == null)
                {
                    ds = Connect.Connect_DataSet("SELECT * FROM Users WHERE User_IsAdmin = False ", "Users");
                    Session["Users"] = ds;
                    dsdefault = ds;
                   
                    
                }
                else
                    ds = (DataSet)Session["Users"];

                try // הצגת חסומים
                {
                    Block = ds.Tables["Users"].Select("User_IsBlocked = true").CopyToDataTable();
                    Session["Block"] = Block;
                    DataList2.DataSource = Block ;
                }
                catch (InvalidOperationException e1)// מקרה בו אין משתמשים חסומים ואז הסינון מחזיר שגיאה
                {
                    DataList2.DataSource = null;
                    Session["Block"] = null;
                }

                DataList2.DataBind();
            }
        }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "DoShow")
            {
                DataTable Block = (DataTable)Session["Block"];
                if (Block == null)
                {
                    Block = ((DataSet)Session["Users"]).Tables["Users"].Clone();
                }
                int selectedIndex = e.Item.ItemIndex; // Index of the selected item in the DataList
                string name = ((Label)e.Item.FindControl("User_Name")).Text;

                if(((Button)e.Item.FindControl("Is_Blocked")).Text == "BLOCK")
                {
                    DataRow newRow = Block.NewRow();
                    DataRow[] rowsToAdd = ((DataSet)Session["Users"]).Tables["Users"].Select("User_Name = '" + name + "'");
                    newRow.ItemArray = rowsToAdd[0].ItemArray;
                    newRow["User_IsBlocked"] = "TRUE";
                    Block.Rows.Add(newRow);
                    Session["Block"] = Block;
                    Connect.Connect_ExecuteNonQuery("UPDATE Users SET User_IsBlocked = true WHERE User_Name= '" + name + "'");
                    DataList2.DataSource = Block;
                    DataList2.DataBind();
                    ((Label)e.Item.FindControl("User_IsBlocked")).Text = "חסום";
                    ((Button)e.Item.FindControl("Is_Blocked")).Text = "UNBLOCK";
                }
                else
                {
                    Block = (DataTable)Session["Block"];
                    DataRow[] rowsToRemove = Block.Select("User_Name = '" + name + "'");
                    foreach (DataRow row in rowsToRemove)
                        Block.Rows.Remove(row);
                    Session["Block"] = Block;
                    Connect.Connect_ExecuteNonQuery("UPDATE Users SET User_IsBlocked = false WHERE User_Name= '" + name + "'");
                    DataList2.DataSource = Block;
                    DataList2.DataBind();
                    ((Label)e.Item.FindControl("User_IsBlocked")).Text = "לא חסום";
                    ((Button)e.Item.FindControl("Is_Blocked")).Text = "BLOCK";
                }

            }
        }
        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string IsBlocked = ((DataSet)Session["Users"]).Tables["Users"].Rows[e.Item.ItemIndex]["User_IsBlocked"].ToString();
                if (IsBlocked == "False")
                {
                    ((Label)e.Item.FindControl("User_IsBlocked")).Text = "לא חסום";
                    ((Button)e.Item.FindControl("Is_Blocked")).Text = "BLOCK";
                }
                else
                {
                    ((Label)e.Item.FindControl("User_IsBlocked")).Text = "חסום";
                    ((Button)e.Item.FindControl("Is_Blocked")).Text = "UNBLOCK";
                }
            }
        }

        protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "DoShow")
            {
                DataTable Block = (DataTable)Session["Block"];
                int selectedIndex = e.Item.ItemIndex; // Index of the selected item in the DataList
                string name = ((Label)e.Item.FindControl("User_Name")).Text;
                DataRow[] rowsToRemove = Block.Select("User_Name = '" + name + "'");
                foreach (DataRow row in rowsToRemove)
                    Block.Rows.Remove(row);
                Session["Block"] = Block;
                Connect.Connect_ExecuteNonQuery("UPDATE Users SET User_IsBlocked = false WHERE User_Name= '" + name + "'");
                DataList2.DataSource = Block;
                DataList2.DataBind();
                if(DataList1.Items.Count != 0 && ((Label)DataList1.Items[0].FindControl("User_Name")).Text == name)
                {
                    ((Label)DataList1.Items[0].FindControl("User_IsBlocked")).Text = "לא חסום";
                    ((Button)DataList1.Items[0].FindControl("Is_Blocked")).Text = "BLOCK";
                }    
            }
        }
        
        protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string IsBlocked = ((DataTable)Session["Block"]).Rows[e.Item.ItemIndex]["User_IsBlocked"].ToString();
                if (IsBlocked == "False")
                {
                    ((Label)e.Item.FindControl("User_IsBlocked")).Text = "לא חסום";
                    ((Button)e.Item.FindControl("Is_Blocked")).Text = "BLOCK";
                }
                else
                {
                    ((Label)e.Item.FindControl("User_IsBlocked")).Text = "חסום";
                    ((Button)e.Item.FindControl("Is_Blocked")).Text = "UNBLOCK";
                }
            }
        }


        protected void SearchBar_TextChanged(object sender, EventArgs e)// חיפוש משתמשים
        {
            try // הצגת משתמש
            {
                DataList1.DataSource = ((DataSet)Session["Users"]).Tables["Users"].Select("User_Name = '" + SearchBar.Text + "'").CopyToDataTable();
            }
            catch (InvalidOperationException e1)// מקרה בו אין משתמשים חסומים ואז הסינון מחזיר שגיאה
            {
                DataList1.DataSource = null;
            }
            DataList1.DataBind();
        }


    }
}
