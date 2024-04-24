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
    public partial class Attraction_Display : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataSet dsdefault = null; //ללא שום סינון  DataSet ה 
            if (!Page.IsPostBack)
            {
                if (Session["Attraction"] == null)
                {
                    ds = Connect.Connect_DataSet("SELECT Attraction_ID, Attraction_Name, Attraction_Type, Attraction_MinAge," +
                        " Attraction_MaxAge, Attraction_Address, Attraction_Text, Attraction_Photo, Attraction_Latitude," +
                        " Attraction_Longitude, Attraction_PathOrder, Attraction_AddDate, VacationType_ID FROM Attraction" +
                        " INNER JOIN AttractionType ON Attraction.Attraction_Type = AttractionType.AttractionType_ID ", "Attraction");
                    Session["AttractionDS"] = ds;
                    dsdefault = ds;

                }
                else
                    ds = (DataSet)Session["Attraction"];

                DataList1.DataSource = ds;
                

                ArrayList arr_DSO = new ArrayList();
                arr_DSO.Add(new DS_Object("SELECT * FROM Attraction INNER JOIN OwnedAttraction ON Attraction.Attraction_ID = OwnedAttraction.Attraction_ID  ", "OwnedAttraction"));
                arr_DSO.Add(new DS_Object("SELECT * FROM Attraction INNER JOIN NatureAttraction ON Attraction.Attraction_ID = NatureAttraction.Attraction_ID  ", "NatureAttraction"));
                DataSet innerds = Connect.Connect_MultipleDataSet(arr_DSO);
                Session["FullAttractions"] = innerds;

                DataList1.DataBind();

                ArrayList AttractionType = Connect.FillArrayListForDropDownList("SELECT AttractionType_ID, AttractionType_Type FROM AttractionType ;", "AttractionType_Type", "AttractionType_ID");
                AddItemsToCheckBoxList(AttractionType, CheckBoxList1);//CheckBoxList  הוספת ערכים ל 

                //שינוי צבע הכפתור - "הכל" לנבחר
            }
        }

        //CheckBoxList ועצם- פקד ListItem עם ערכים מסוג ArrayList מקבל  .CheckBoxList מוסיף ערכים ל 
        public static void AddItemsToCheckBoxList(ArrayList arr, CheckBoxList cbl)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                cbl.Items.Add((ListItem)arr[i]);
            }
        }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //Move to another page to show the product
            if (e.CommandName == "DoShow")
            {
                //((DataSet)Session["AttractionDS"]).Tables[0].Rows[e.Item.ItemIndex]["Attraction_ID"].ToString();
                Session["AttractionID_ForAttractionPage"] = ((DataSet)Session["AttractionDS"]).Tables[0].Rows[e.Item.ItemIndex]["Attraction_ID"].ToString();
            }
            Session["from"] = "Attraction_Display.aspx";
            Response.Redirect("Attraction_Page.aspx");
        }

        protected void Button_All_Click(object sender, EventArgs e)
        {
            string s = "SELECT * FROM attraction INNER JOIN AttractionType ON Attraction.Attraction_Type = AttractionType.AttractionType_ID ";
            CheckBoxList1.SelectedIndex = -1;
            Min_Age.Text = "";
            Max_Age.Text = "";
            Price.Text = "";
            Duration.Text = "";
            Attraction_FreeEntry.SelectedIndex = 0;
            DataSet ds = Connect.Connect_DataSet(s, "Attraction");
            Session["AttractionDS"] = ds;
            DataList1.DataSource = (DataSet)Session["AttractionDS"];
            DataList1.DataBind();
        }

        protected void filter_Click1(object sender, EventArgs e)
        {
            string s = "SELECT * FROM attraction INNER JOIN AttractionType ON Attraction.Attraction_Type = AttractionType.AttractionType_ID ";
            for (int i = 0; i < CheckBoxList1.Items.Count; i++)// בדיקת סימון בצקבוקסליסט - סינון לפי אטרקציה
            {
                if (CheckBoxList1.Items[i].Selected)
                {
                    if (!s.Contains("WHERE"))
                        s += "WHERE Attraction_Type =";

                    s += CheckBoxList1.Items[i].Value + " OR Attraction_Type= ";
                }
            }
            if (s.Contains("WHERE"))// אם סינן לפי אטרקציה מסדר את המחרוזת
                s = s.Substring(0, s.Length - 21);


            if (Min_Age.Text != "" && Max_Age.Text == "")// בדיקת סינון גילים 
            {
                if (s.Contains("WHERE"))
                    s += " AND ";
                else
                    s += " WHERE ";
                s += "Attraction_MinAge BETWEEN " + Min_Age.Text + " AND 120";
            }
            else if (Min_Age.Text == "" && Max_Age.Text != "")
            {
                if (s.Contains("WHERE"))
                    s += " AND ";
                else
                    s += " WHERE ";
                s += "Attraction_MaxAge BETWEEN 0 AND " + Max_Age.Text;
            }
            else if (Min_Age.Text != "" && Max_Age.Text != "")
            {
                if (s.Contains("WHERE"))
                    s += " AND ";
                else
                    s += " WHERE ";
                s += "Attraction_MaxAge BETWEEN " + Min_Age.Text + " AND " + Max_Age.Text + " AND Attraction_MinAge BETWEEN " + Min_Age.Text + " AND " + Max_Age.Text;
            }

            if (Price.Text != "")// סינון לפי מחיר
            {
                if (s.Contains("WHERE"))
                    s += " AND ";
                else
                    s += " WHERE ";
                s += "Attraction_Price = " + Price.Text;
            }

            if (Duration.Text != "")// סינון לפי משך אטרקציה
            {
                if (s.Contains("WHERE"))
                    s += " AND ";
                else
                    s += " WHERE ";
                s += "Attraction_Duration = " + Duration.Text;
            }

            if (Attraction_FreeEntry.SelectedValue != "האם כניסה חופשית")// סינון לפי כניסה חופשית
            {
                if (s.Contains("WHERE"))
                    s += " AND ";
                else
                    s += " WHERE ";
                s += "Attraction_FreeEntry = " + Attraction_FreeEntry.SelectedValue;
            }
            Session["AttractionDS"] = Connect.Connect_DataSet(s, "Attraction");
            //DataSet ds = (DataSet)Session["AttractionDS"];
            //if (true)
            //{
            //    //על פי עמודה DataSet מיון - סידור - ORDER BY על DataSet
            //    DataView dataView = new DataView(ds.Tables["Attraction"]);
            //    dataView.Sort = "Attraction_AddDate" + " DESC";// Specify the column you want to order by. If you want to sort in descending order, use: dataView.Sort = columnName + " DESC";
            //    DataTable sortedDataTable = dataView.ToTable();// העברת תוצאת המיון לטבלה חדשה.
            //    DataList1.DataSource = sortedDataTable;
            //}
            //else
            //    DataList1.DataSource = ds;
            DataList1.DataSource = (DataSet)Session["AttractionDS"];
            DataList1.DataBind();
        }

        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int IdNum = Convert.ToInt32(((Label)e.Item.FindControl("Attraction_ID")).Text);
                int TypeNum = Convert.ToInt32(((Label)e.Item.FindControl("Attraction_VacationType_ID")).Text);
                if (TypeNum != 3)
                {
                    foreach (DataRow row in ((DataSet)Session["FullAttractions"]).Tables["OwnedAttraction"].Rows)
                    {
                        if ((int)row["Attraction.Attraction_ID"] == IdNum)
                        {
                            ((Label)e.Item.FindControl("PriceOrKilometers")).Text = "שח" + row["OwnedAttraction_Price"].ToString();
                            break;
                        }
                    }
                }
                else
                {
                    foreach (DataRow row in ((DataSet)Session["FullAttractions"]).Tables["NatureAttraction"].Rows)
                    {
                        if ((int)row["Attraction.Attraction_ID"] == IdNum)
                        {
                            ((Label)e.Item.FindControl("PriceOrKilometers")).Text = "קילומטר" + row["NatureAttraction_KilometersNumber"].ToString();
                            break;
                        }
                    }
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["from"].ToString());
        }

        //protected void CheckBoxList2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    foreach (ListItem item in Attraction_OrderByDate.Items)
        //    {
        //        if (item.Value != Attraction_OrderByDate.SelectedValue)
        //        {
        //            item.Selected = false;
        //        }
        //    }

        //}
    }
}