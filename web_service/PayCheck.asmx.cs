using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;

namespace web_service
{
    /// <summary>
    /// Summary description for PayCheck
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PayCheck : System.Web.Services.WebService
    {

        [WebMethod]
        public bool Pay(string Number, string Owner_ID, string Provider, string CVV, string DateOfExpiration, string Cost, int Company)
        {
            CreditCard card = null;

            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = GetConnectionString();
            string s = "SELECT DateOfExpiration, Balance FROM Credit_Card WHERE Number = '" + Number + "' AND Owner_ID = '"
                + Owner_ID + "' AND Provider = '" + Provider + "' AND CVV = '" + CVV + "' ";
            OleDbCommand command = new OleDbCommand(s , Conn);
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                card = new CreditCard(Number, Owner_ID, Provider, CVV, reader["DateOfExpiration"].ToString(), reader["Balance"].ToString());
            }
            Conn.Close();

            if (card == null) // האם קיים כרטיס במסד
                return false;
            if (DateTime.Today > DateTime.Parse(DateOfExpiration))// אם תאריך הכרטיס תקף
                return false;
            if (Convert.ToInt32(card.Balance) < Convert.ToInt32(Cost))// אם יש כסף בחשבון
                return false;

            Connect_ExecuteNonQuery("INSERT INTO Purchase ( [Number] , [Cost], [DateOfPurchase], [Company]) values"  +
                "('" + Number + "', '" + Cost + "', #" + DateTime.Today.ToShortDateString() + "#, " + Company + ")") ;// הוספת עסקה למסד
            Connect_ExecuteNonQuery(" UPDATE Credit_Card SET Balance = '" +
                (Convert.ToInt32(card.Balance) - Convert.ToInt32(Cost)).ToString() + "' WHERE Number = '" + Number + "'"); // עדכון יתרה
            return true;
        }

        public void Connect_ExecuteNonQuery(string s) // ExecuteNonQuery -  הפעלת פעולה במסד הנתונים
        {
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = GetConnectionString();
            OleDbCommand command = new OleDbCommand(s, Conn);
            Conn.Open();
            command.ExecuteNonQuery();
            Conn.Close();
        }

        public string GetConnectionString()
        {
            string FILE_NAME = "Project_WebService.accdb";

            string location = @"C:\Users\yuval\source\repos\Project_01\web_service\Data_Base\Project_WebService.accdb";
            string ConnectionString = @"provider=Microsoft.ACE.OLEDB.16.0; data source=" + location;
            return ConnectionString;
        }
    }
}

