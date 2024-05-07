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
        { //Both ImageButton and LinkButton implement IButtonControl
            Response.Redirect(((IButtonControl)sender).CommandName + ".aspx");
        }
    }
}