using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;


namespace Project_01
{
    public class Connect
    {
        public static string GetConnectionString()
        {
            string FILE_NAME = "Project_01.accdb";

            string location = HttpContext.Current.Server.MapPath("~/App_Data/" + FILE_NAME);
            string ConnectionString = @"provider=Microsoft.ACE.OLEDB.16.0; data source=" + location;
            return ConnectionString;
        }


        public static void Connect_ExecuteNonQuery(string s) // ExecuteNonQuery -  הפעלת פעולה במסד הנתונים
        {
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand(s, Conn);
            Conn.Open();
            command.ExecuteNonQuery();
            Conn.Close();
        }
        public static object Connect_ExecuteScalar(string s) // ExecuteScalar - הפעלת פעולה במסד הנתונים 
        {
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand(s, Conn);
            Conn.Open();
            object obj = (object)command.ExecuteScalar();
            Conn.Close();
            return obj;
        }

        public static DataSet Connect_DataSet(string s, string TableName) //  מחזיר דאטה סט המכיל טבלה *אחת* שנקראת בשם מסוים ושנבחרה לפי פקודה מסויימת 
        {
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand(s, Conn); ;
            DataSet ds = new DataSet();
            OleDbDataAdapter adapter;
            Conn.Open();
            adapter = new OleDbDataAdapter(command);
            DataTable dt1 = new DataTable(TableName);
            adapter.Fill(dt1);
            ds.Tables.Add(dt1);
            Conn.Close();
            return ds;
        }

        public static DataTable Connect_DataTable(string s, string TableName)//  מחזיר טבלה *אחת* שנקראת בשם מסוים ושנבחרה לפי פקודה מסויימת
        {
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand(s, Conn);
            DataTable dt = new DataTable(TableName);
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);

            Conn.Open();
            adapter.Fill(dt);
            Conn.Close();

            return dt;
        }

        public static DataSet Connect_MultipleDataSet(ArrayList arr_DSO)//בעלי שאילתה ושם טבלה - מחזיר דאטאסט המכיל טבלה אחת *לפחות* שנקראת בשם מסויים ושנבחרה לפי פקודה מסויימת DS_Object המכיל עצמי ArrayList מקבל
        {
            OleDbConnection Conn = new OleDbConnection(); 
            Conn.ConnectionString = Connect.GetConnectionString();  
            DataSet ds = new DataSet();
            OleDbCommand command;
            OleDbDataAdapter adapter;
            Conn.Open();
            for (int i = 0; i < arr_DSO.Count; i++)// הנחה שהארייליסט גדול מאפס
            {
                command = new OleDbCommand( ((DS_Object)arr_DSO[i]).s , Conn);
                adapter = new OleDbDataAdapter(command);
                DataTable dt = new DataTable(((DS_Object)arr_DSO[i]).TableName);
                adapter.Fill(dt);
                ds.Tables.Add(dt);
            }
            Conn.Close();
            return ds;
        }

        public static ArrayList FillArrayListForDropDownList(string s, string firstColumn, string SecondColumn) //  השמת נתונים מתוך מסד הנתונים בתןך ארייליסט - מקבל שאילתה, שם עמודה ראשונה(הטקסט שיוצג) ושניה(הערך) (מתוך תוצאות השאילתה) - מחזיר ארייליסט 
        {
            ArrayList arr = new ArrayList();
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand(s, Conn);
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                arr.Add(new ListItem(reader[firstColumn].ToString(), reader[SecondColumn].ToString()));
            }
            return arr;
        }
        //Attraction_Type.Items.Add(new ListItem("ok","1"));// ok = הטקסט המוצג 
        //Attraction_Type.Items.Add(new ListItem("hi", "2"));// המשך - 1 = הערך , אם אין ערך הטקסט המוצג הוא הערך כבררית מחדל
    }
}