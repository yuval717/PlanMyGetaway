using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_01
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Menu_Click(object sender, EventArgs e) //sender is which element caused the event
        {
            Attractions_Display.filterdTableForAttractoinPage = null;
            //Both ImageButton and LinkButton implement IButtonControl
            Response.Redirect(((IButtonControl)sender).CommandName + ".aspx");
        }

        public ImageButton MasterPageLogo
        {
            get { return MasterPage_Logo; }
        }

        public LinkButton MasterPageSignUpOut
        {
            get { return MasterPage_sign_Up_Out; }
        }

        public LinkButton MasterPageAbout
        {
            get { return MasterPage_About; }
        }

        public LinkButton MasterPageOrders
        {
            get { return MasterPage_Orders; }
        }

        public LinkButton MasterPageNewOrder
        {
            get { return MasterPage_NewOrder; }
        }

        protected void MasterPage_sign_Up_Out_Click(object sender, EventArgs e)
        {
            if (MasterPageSignUpOut.Text == "התנתק")
            {
                Session.Clear(); // מחיקת כל הסשנים

            }
            Attractions_Display.filterdTableForAttractoinPage = null;
            Response.Redirect(((IButtonControl)sender).CommandName + ".aspx");
        }
    }
}