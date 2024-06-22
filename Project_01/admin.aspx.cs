using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

namespace Project_01
{
    public partial class admin : System.Web.UI.Page
    {
        private DataSet ds = null;
        private DataSet dsdefault = null; //ללא שום סינון  DataSet ה 
        private DataTable Block = new DataTable(); //חסומים
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
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

                Session["Users"] = null;//עדכון תצוגה
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
                    DataList2.DataSource = Block;
                }
                catch (InvalidOperationException e1)// מקרה בו אין משתמשים חסומים ואז הסינון מחזיר שגיאה
                {
                    DataList2.DataSource = null;
                    Session["Block"] = null;
                }

                DataList2.DataBind();

                if (DataList2.DataSource == null)
                {
                    BlockedUsers_Lalbe.Visible = false;
                }
                //סוגי חופשות
                ArrayList arr1 = Connect.FillArrayListForDropDownList("SELECT VacationType_Type ,VacationType_ID FROM VacationType WHERE IsValid = " + true +"", "VacationType_Type", "VacationType_ID");
                for (int i = 0; i < arr1.Count; i++) //שהחזירה עצמי דרופדאוןליסט FillArrayListForDropDownList שקיבלנו מהפעלת הפעולה  ArrayList הכנסת ערכי מדינות (מזהה ושם מדינה) לדרופדאוןליסט מתוך  
                {
                    VactionType_Add_DropDownList.Items.Add((ListItem)arr1[i]);
                }

                ArrayList arr2 = Connect.FillArrayListForDropDownList("SELECT AttractionType_Type ,AttractionType_ID FROM AttractionType WHERE IsValid = "+true +"", "AttractionType_Type", "AttractionType_ID");
                for (int i = 0; i < arr2.Count; i++) //שהחזירה עצמי דרופדאוןליסט FillArrayListForDropDownList שקיבלנו מהפעלת הפעולה  ArrayList הכנסת ערכי מדינות (מזהה ושם מדינה) לדרופדאוןליסט מתוך  
                {
                    AttractionType_Remove_DropDownList.Items.Add((ListItem)arr2[i]);
                }

                for (int i = 0; i < arr2.Count; i++) //שהחזירה עצמי דרופדאוןליסט FillArrayListForDropDownList שקיבלנו מהפעלת הפעולה  ArrayList הכנסת ערכי מדינות (מזהה ושם מדינה) לדרופדאוןליסט מתוך  
                {
                    AttractionType_Edit_DropDownList.Items.Add((ListItem)arr2[i]);
                }

                ArrayList arr3 = Connect.FillArrayListForDropDownList("SELECT VacationType_Type ,VacationType_ID FROM VacationType WHERE ( VacationType_ID <> "+1+" AND VacationType_ID <> "+2+" AND VacationType_ID <> "+3+" AND VacationType_ID <> "+4+" ) AND IsValid = " + true + "", "VacationType_Type", "VacationType_ID");

                for (int i = 0; i < arr3.Count; i++) //שהחזירה עצמי דרופדאוןליסט FillArrayListForDropDownList שקיבלנו מהפעלת הפעולה  ArrayList הכנסת ערכי מדינות (מזהה ושם מדינה) לדרופדאוןליסט מתוך  
                {
                    VacationType_ToRemove_DropDownList.Items.Add((ListItem)arr3[i]);
                }

                for (int i = 0; i < arr3.Count; i++) //שהחזירה עצמי דרופדאוןליסט FillArrayListForDropDownList שקיבלנו מהפעלת הפעולה  ArrayList הכנסת ערכי מדינות (מזהה ושם מדינה) לדרופדאוןליסט מתוך  
                {
                    VacationType_ToEdit_DropDownList.Items.Add((ListItem)arr3[i]);
                }


            }
        }

        //חסימת/שחרור/מעבר לפרטי משתמש
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

                if (((Button)e.Item.FindControl("Is_Blocked")).Text == "BLOCK")
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

                    //אם אין חסומים - אל תציג כותרת חסומים ואם יש הצג/המשך להציג
                    if (Block.Rows.Count == 0)
                        BlockedUsers_Lalbe.Visible = false;
                    else
                        BlockedUsers_Lalbe.Visible = true;
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

                    //אם אין חסומים - אל תציג כותרת חסומים ואם יש הצג/המשך להציג
                    if (Block.Rows.Count == 0)
                        BlockedUsers_Lalbe.Visible = false;
                    else
                        BlockedUsers_Lalbe.Visible = true;
                }

            }
            if(e.CommandName == "Move")
            {
                if(((Button)e.Item.FindControl("User_VacationsOrAttractions")).Text == "חופשות המשתמש")
                {
                    Session["UserOfWatch"] = ((Label)e.Item.FindControl("User_Name")).Text;
                    Response.Redirect("User_Folder/UsersOrders.aspx"); // העברה לדף יצירת הזמנה
                }
                else
                {
                    Session["UserOfWatch"] = ((Label)e.Item.FindControl("User_Name")).Text ;
                    Response.Redirect("Attraction_Folder/Attraction_Owner.aspx"); // 
                }
            }
        }

        //דאטאבאונד -משתמשים
        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                string IsBlocked = ((Label)e.Item.FindControl("User_IsBlocked")).Text;
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
                string IsAttractionOwner = ((Label)e.Item.FindControl("User_isAttractionOwner")).Text;
                if (IsAttractionOwner == "True")
                    ((Button)e.Item.FindControl("User_VacationsOrAttractions")).Text = "אטרקציות בבעלות המשתמש";
                ((Label)e.Item.FindControl("User_LastEntrance")).Text = (((Label)e.Item.FindControl("User_LastEntrance")).Text).Substring(0, 10);
            }
        }

        //חסימת/שחרור/מעבר לפרטי משתמש - משתמשים חסומים
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
                if (DataList1.Items.Count != 0 && ((Label)DataList1.Items[0].FindControl("User_Name")).Text == name)
                {
                    ((Label)DataList1.Items[0].FindControl("User_IsBlocked")).Text = "לא חסום";
                    ((Button)DataList1.Items[0].FindControl("Is_Blocked")).Text = "BLOCK";
                }

                //אם אין חסומים - אל תציג כותרת חסומים ואם יש הצג/המשך להציג
                if (Block.Rows.Count == 0)
                    BlockedUsers_Lalbe.Visible = false;
                else
                    BlockedUsers_Lalbe.Visible = true;
            }

            if (e.CommandName == "Move")
            {
                if (((Button)e.Item.FindControl("User_VacationsOrAttractions")).Text == "חופשות המשתמש")
                {
                    Session["UserOfWatch"] = ((Label)e.Item.FindControl("User_Name")).Text;
                    Response.Redirect("User_Folder/UsersOrders.aspx"); // העברה לדף יצירת הזמנה
                }
                else
                {
                    string k = ((Label)e.Item.FindControl("User_Name")).Text;
                    Session["UserOfWatch"] = ((Label)e.Item.FindControl("User_Name")).Text;
                    Response.Redirect("Attraction_Folder/Attraction_Owner.aspx"); // 
                }
            }


        }

        //דאטאבאונד - משתמשים חסומים
        protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string IsBlocked = ((Label)e.Item.FindControl("User_IsBlocked")).Text;
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
                string IsAttractionOwner = ((Label)e.Item.FindControl("User_isAttractionOwner")).Text;
                if (IsAttractionOwner == "True")
                    ((Button)e.Item.FindControl("User_VacationsOrAttractions")).Text = "אטרקציות בבעלות המשתמש";
                ((Label)e.Item.FindControl("User_LastEntrance")).Text = (((Label)e.Item.FindControl("User_LastEntrance")).Text).Substring(0,10);
                
            }
        }

        //חיפוש משתמש
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

        //טיפול בדיב הוספת סוג אטרקציה
        protected void AttractionType_Add_Click(object sender, EventArgs e)
        {
            Result_Att.Text = "";
            AddDivAttraction.Style["display"] = "block";
            RemoveDivAttraction.Style["display"] = "none";
            EditDivAttraction.Style["display"] = "none";

            //סגירת דיבים פתוחים של סוג חופשה
            AddDivVacation.Style["display"] = "none";
            RemoveDivVacation.Style["display"] = "none";
            EditDivVacation.Style["display"] = "none";
        }

        //טיפול בדיב מחיקת סוג אטרקציה
        protected void AttractionType_Remove_Click(object sender, EventArgs e)
        {
            Result_Att.Text = "";
            RemoveDivAttraction.Style["display"] = "block";
            AddDivAttraction.Style["display"] = "none";
            EditDivAttraction.Style["display"] = "none";

            //סגירת דיבים פתוחים של סוג חופשה
            AddDivVacation.Style["display"] = "none";
            RemoveDivVacation.Style["display"] = "none";
            EditDivVacation.Style["display"] = "none";
        }

        //טיפול בדיב עריכת סוג אטרקציה
        protected void AttractionType_Edit_Click(object sender, EventArgs e)
        {
            Result_Att.Text = "";
            EditDivAttraction.Style["display"] = "block";
            AddDivAttraction.Style["display"] = "none";
            RemoveDivAttraction.Style["display"] = "none";

            //סגירת דיבים פתוחים של סוג חופשה
            AddDivVacation.Style["display"] = "none";
            RemoveDivVacation.Style["display"] = "none";
            EditDivVacation.Style["display"] = "none";
        }

        // הוספת סוג אטרקציה
        protected void AttractionType_Add_Button_Click(object sender, EventArgs e)
        {
            if (Connect.Connect_ExecuteScalar("Select * From AttractionType Where AttractionType_Type = '" + AttractionType_Add_TextBox.Text + "'") == null)
                Connect.Connect_ExecuteNonQuery("INSERT INTO AttractionType (AttractionType_Type, VacationType_ID, IsValid) VALUES ('" + AttractionType_Add_TextBox.Text + "', " + VactionType_Add_DropDownList.SelectedValue + ", "+ true+");");
            Response.Redirect("Admin.aspx");
        }

        // עריכת סוג אטרקציה
        protected void AttractionType_Edit_Button_Click(object sender, EventArgs e)
        {
            if (AttractionType_Edit_DropDownList.SelectedValue != null)
            {
                Connect.Connect_ExecuteNonQuery("Update AttractionType Set AttractionType_Type = '" + AttractionType_Edit_TextBox.Text + "' WHERE AttractionType_ID = " + AttractionType_Edit_DropDownList.SelectedValue);
                Response.Redirect("Admin.aspx");
            }
            else
                Result_Att.Text = "בחר סוג אטרקציה תקין לעריכה";

        }

        // מחיקת סוג אטרקציה
        protected void AttractionTypeRemove_Button_Click(object sender, EventArgs e)
        {
            if (AttractionType_Remove_DropDownList.SelectedValue != "")
            {
                Connect.Connect_ExecuteNonQuery("Update AttractionType Set IsValid = " + false + " WHERE AttractionType_ID = " + AttractionType_Remove_DropDownList.SelectedValue);
                Response.Redirect("Admin.aspx");
            }
            else
                Result_Att.Text = "בחר סוג אטרקציה תקין למחיקה";

        }

        //טיפול בדיב הוספת סוג חופשה
        protected void VacationType_Add_Click(object sender, EventArgs e)
        {
            Result_Att.Text = "";
            AddDivVacation.Style["display"] = "block";
            RemoveDivVacation.Style["display"] = "none";
            EditDivVacation.Style["display"] = "none";

            //סגירת דיבים פתוחים של סוג אטרקציה
            EditDivAttraction.Style["display"] = "none";
            AddDivAttraction.Style["display"] = "none";
            RemoveDivAttraction.Style["display"] = "none";
        }

        //טיפול בדיב עריכת סוג חופשה
        protected void VacationType_Edit_Click(object sender, EventArgs e)
        {
            Result_Att.Text = "";
            EditDivVacation.Style["display"] = "block";
            AddDivVacation.Style["display"] = "none";
            RemoveDivVacation.Style["display"] = "none";

            //סגירת דיבים פתוחים של סוג אטרקציה
            EditDivAttraction.Style["display"] = "none";
            AddDivAttraction.Style["display"] = "none";
            RemoveDivAttraction.Style["display"] = "none";
        }

        //טיפול בדיב הסרת סוג חופשה
        protected void VacationType_Remove_Click(object sender, EventArgs e)
        {
            Result_Att.Text = "";
            RemoveDivVacation.Style["display"] = "block";
            AddDivVacation.Style["display"] = "none";
            EditDivVacation.Style["display"] = "none";

            //סגירת דיבים פתוחים של סוג אטרקציה
            EditDivAttraction.Style["display"] = "none";
            AddDivAttraction.Style["display"] = "none";
            RemoveDivAttraction.Style["display"] = "none";
        }

        //הוספת סוג חופשה
        protected void VacationTypeAdd_Button_Click(object sender, EventArgs e)
        {
            if (Connect.Connect_ExecuteScalar("Select * From VacationType Where VacationType_Type = '" + VacationType_ToAdd_Drop.Text + "'") == null)
                Connect.Connect_ExecuteNonQuery("INSERT INTO VacationType (VacationType_Type, IsValid) VALUES ('" + VacationType_ToAdd_Drop.Text + "', " + true + ");");
            Response.Redirect("Admin.aspx");
        }

        //הסרת סוג חופשה
        protected void VacationTypeRemove_Button_Click(object sender, EventArgs e)
        {
            if (VacationType_ToRemove_DropDownList.SelectedValue != "")
            {
                Connect.Connect_ExecuteNonQuery("Update VacationType Set IsValid = " + false + " WHERE VacationType_ID = " + VacationType_ToRemove_DropDownList.SelectedValue);
                Response.Redirect("Admin.aspx");
            }
            else
                Result_Att.Text = "בחר סוג חופשה תקין למחיקה";

        }

        //עריכת סוג חופשה
        protected void VacationEditAdd_Button_Click(object sender, EventArgs e)
        {
            if (VacationType_ToEdit_DropDownList.SelectedValue != "")
            {
                Connect.Connect_ExecuteNonQuery("Update VacationType Set VacationType_Type = '" + VacationType_ToEdit.Text + "' WHERE VacationType_ID = " + VacationType_ToEdit_DropDownList.SelectedValue);
                Response.Redirect("Admin.aspx");
            }
            else
                Result_Att.Text = "בחר סוג חופשה תקין לעריכה";
        }
    }
}