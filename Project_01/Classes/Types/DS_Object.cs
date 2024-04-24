using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;

namespace Project_01
{
    public class DS_Object// מיועד לדאטא סאט המכיל טבלאות מרובות
    {
        public string s { get; set; } // שאילתה - ליבוא טבלה
        public string TableName { get; set; } // שם הטבלה
        public DS_Object (string s, string TablesNames)
        {
            this.s = s;
            this.TableName = TablesNames;
        }
    }
}