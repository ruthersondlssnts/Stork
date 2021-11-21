using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using NLog;
using _1ClickDelivery.Models;

namespace _1ClickDelivery.UserClasses
{
    public class EmailHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public enum TranType
        {
            Schedule,
            Waybill
        }

        public string SendEmail(TranType type, string createdBy, string number, string body)
        {
            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            try
            {
                var subject = string.Empty;
                if (type == TranType.Schedule)
                {
                    subject = "New PICKUP SCHEDULE - " + createdBy + " - " + number;
                }
                else if (type == TranType.Waybill)
                {
                    subject = "New WAYBILL - " + createdBy + " - " + number;
                }

                m.From = new MailAddress("info@stock.ph");
                m.To.Add("rutherson20@gmail.com");
                m.Subject = subject;
                m.IsBodyHtml = true;
                m.Body = body;
                sc.Host = "smtp.gmail.com";
                sc.Port = 587;
                sc.Credentials = new System.Net.NetworkCredential("rutherson20@gmail.com", "rds1202000");

                //sc.EnableSsl = true;
                sc.Send(m);
                return "Email Send successfully";
            }
            catch (Exception ex)
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Logs.Add(new Log { PKLog = Guid.NewGuid(), DateTimeCreated = DateTime.Now, CreatedBy = "System", Message = ex.Message + " - " + ex.InnerException });
                    db.SaveChanges();
                }
                logger.Debug(ex.Message + " " + ex.InnerException);
                return ex.Message;
            }
        }


    }
}