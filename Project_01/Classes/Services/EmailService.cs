using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;


namespace Project_01
{
    //האימייל לא עובד
    public class EmailService
    {
        public EmailService()
        { }
        public static void SendEmail(string body, string subject, string EmailAddress)
        {
            //יצירת אוביקט MailMessage
            MailMessage mail = new MailMessage();

            //למי לשלוח (יש אפשרות להוסיף כמה נמענים) 
            mail.To.Add(EmailAddress);

            //כתובת מייל לשלוח ממנה
            mail.From = new MailAddress("yuvalaharoni628@gmail.com");

            // נושא ההודעה
            mail.Subject = subject;

            //תוכן ההודעה ב- HTML
            mail.Body = body;

            //הגדרת תוכן ההודעה ל - HTML 
            mail.IsBodyHtml = true;

            // Smtp יצירת אוביקט 
            SmtpClient smtp = new SmtpClient();

            //הגדרת השרת של גוגל
            smtp.Host = "smtp.gmail.com";

            //הגדרת פרטי הכניסה לחשבון גימייל
            smtp.Credentials = new System.Net.NetworkCredential("oritdueivbazak@gmail.com", "");
            //אפשור SSL
            smtp.EnableSsl = true;

            //שליחת ההודעה
            smtp.Send(mail);
        }

    }
}