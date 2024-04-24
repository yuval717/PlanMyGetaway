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
    public partial class Attraction_Page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(! Page.IsPostBack)
            {
                //Session["AttractionID_ForAttractionPage"]= "28";
                Attraction a = AttractionService.FillAttraction("SELECT * FROM Attraction INNER JOIN AttractionType ON Attraction.Attraction_Type = AttractionType.AttractionType_ID WHERE Attraction.Attraction_ID =" + Session["AttractionID_ForAttractionPage"].ToString() );
                Attraction_Name.Text = "שם האטרקציה :" + a.Attraction_Name  ;
                Attraction_Type.Text = "סוג האטרקציה : " + a.Attraction_TypeName;
                Attraction_Age.Text = "טווח גילאי האטרקציה :" + a.Attraction_MaxAge + " - " + a.Attraction_MinAge ;
                //Attraction_Price.Text = "מחירי האטרקציה : $" + a.Attraction_Price ;
                //Attraction_Duration.Text =" משך האטרקציה : "+ a.Attraction_Duration;
                Attraction_Address.Text = "כתובת האטרקציה : " +a.Attraction_Address;
                //Attraction_Gmail.Text =  a.Attraction_Gmail+ " : אימייל האטרקציה";
                //Attraction_PhonNumber.Text = "מספר טלפון האטרקציה : "+ a.Attraction_PhonNumber;
                Attraction_Text.Text = "מידע על האטרציה : "+ a.Attraction_Text;
                Image1.ImageUrl = a.Attraction_Photo;
                Session["Attraction"] = a;
            }
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["from"].ToString());
        }
    }
}