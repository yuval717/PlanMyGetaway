using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using web_service;
using System.Data;

namespace Project_01
{
    public partial class Pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int Order_ID = 1;
            string queryString = @"SELECT Attraction.Attraction_ID, Attraction.Attraction_Type FROM ((([Attraction] 
                       INNER JOIN [Day_Attraction] ON [Attraction].[Attraction_ID] = [Day_Attraction].[Attraction_ID]) 
                       INNER JOIN [Day] ON [Day_Attraction].[Day_ID] = [Day].[Day_ID]) 
                       INNER JOIN [Orders] ON [Day].[Order_ID] = [Orders].[Order_ID]) 
                       WHERE [Orders].[Order_ID] = " + Order_ID + ";";
            DataSet ds = Connect.Connect_DataSet(queryString, "Attractions");
            DataSet Nature_Attractions = Connect.Connect_DataSet("Select AttractionType_ID From AttractionType Where VacationType_ID = 3", "Attractions");
            // לרוץ על הדאטא סט דיאס ולבדוק אם הטייפ שווה לטייפ נייטר אם לא אז לבדוק מחיר
        }

        protected void enter_Click(object sender, EventArgs e)
        {
            PayCheck p = new PayCheck();
            p.Pay(Number.Text, Owner_ID.Text, Provider.SelectedValue, CVV.Text, DateOfExpiration.Text, "55", 1);
        }
    }
}