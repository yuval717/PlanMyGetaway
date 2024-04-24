using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;

namespace WebService_CreditCard
{
    /// <summary>
    /// Summary description for Payment
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Payment : System.Web.Services.WebService
    {

        [WebMethod]
        public bool Pay(string Number, string Owner_ID, string Provider, string CVV, string DateOfExpiration, string Cost, string Company)
        {
            Credit_Card card = null;

            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = GetConnectionString();
            OleDbCommand command = new OleDbCommand("SELECT * FROM Credit_Card WHERE Number = '" + Number + "' AND Owner_ID = '"
                + Owner_ID + "' AND Provider = '" + Provider + "' AND CVV = '" + CVV + "' AND DateOfExpiration = '"
                + DateOfExpiration + "'", Conn);
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                card = new Credit_Card(reader["Number"].ToString(), reader["Owner_ID"].ToString(), reader["Provider"].ToString(),
                    reader["CVV"].ToString(), reader["DateOfExpiration"].ToString(), reader["Balance"].ToString());
            }
            Conn.Close();

            if (card == null) // האם קיים כרטיס במסד
                return false;
            if (DateTime.Today < DateTime.Parse(DateOfExpiration))// אם תאריך הכרטיס תקף
                return false;
            if (Convert.ToInt32(card.Balance) < Convert.ToInt32(Cost))// אם יש כסף בחשבון
                return false;

            Connect_ExecuteNonQuery(" INSERT INTO Purchase (Number, Cost, DateOfPurchase, Company)" +
                " VALUES ('" + Number + "', '" + Cost + "', '" + DateTime.Today + "', '" + Company + "'");// הוספת עסקה למסד
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

            string location = "C:\Users\yuval\source\repos\Project_01\WebService_CreditCard\App_Data\Project_WebService.accdb";
            string ConnectionString = @"provider=Microsoft.ACE.OLEDB.16.0; data source=" + location;
            return ConnectionString;
        }
    }
}

