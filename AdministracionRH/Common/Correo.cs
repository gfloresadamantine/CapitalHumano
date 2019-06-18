using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace AdministracionRH.Common
{
    public class Correo
    {
        public void SendMail(List<MailAddress> Para, string Subject, string Body)
        {
            try
            {
                string str = ConfigurationManager.AppSettings["userfrom"];
                string str2 = ConfigurationManager.AppSettings["smtpserver"];
                SmtpClient client = new SmtpClient();
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(str)
                };
                foreach (MailAddress address in Para)
                {
                    message.To.Add(address);
                }
                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.Body = Body;
                client.Host = str2;
                client.Send(message);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<Attachment> AttachFile(string strfilepath)
        {
            List<Attachment> listaAttach = new List<Attachment>();
            FileInfo file = new FileInfo(strfilepath);
            if (file.Exists)
            {
                Attachment attach = new Attachment(strfilepath);
                listaAttach.Add(attach);
            }
            return listaAttach;

        }
        public void SendMail(List<MailAddress> listacorreos, string subject, string body, List<Attachment> listaarchivos)
        {
            try
            {
                string strFrom = ConfigurationManager.AppSettings["userfrom"];
                string strSmtpServer = ConfigurationManager.AppSettings["smtpserver"];

                SmtpClient client = new SmtpClient();
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(strFrom)
                };
                foreach (MailAddress address in listacorreos)
                {
                    message.To.Add(address);
                }
                foreach (Attachment attachment in listaarchivos)
                {
                    message.Attachments.Add(attachment);
                }
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                client.Host = strSmtpServer;
                client.Send(message);
                client.Dispose();
                DisposeAttachments(message);



            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendMailConfirmacion(List<MailAddress> listacorreos, string subject, string body, List<Attachment> listaarchivos)
        {
            try
            {
                string strFrom = ConfigurationManager.AppSettings["userfrom"];
                string strSmtpServer = ConfigurationManager.AppSettings["smtpserver"];

                SmtpClient client = new SmtpClient();
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(strFrom)
                };
                foreach (MailAddress address in listacorreos)
                {
                    message.To.Add(address);
                }
                foreach (Attachment attachment in listaarchivos)
                {
                    message.Attachments.Add(attachment);
                }
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;

                message.DeliveryNotificationOptions =
                 System.Net.Mail.DeliveryNotificationOptions.OnFailure |
                 System.Net.Mail.DeliveryNotificationOptions.OnSuccess |
                 System.Net.Mail.DeliveryNotificationOptions.Delay;

                // Ask for a read receipt
                message.Headers.Add("Disposition-Notification-To", "masterservicing@adamantine.com.mx");

                client.Host = strSmtpServer;
                client.Send(message);
                client.Dispose();
                DisposeAttachments(message);
                //---------------------


            }
            catch (Exception)
            {
                throw;
            }
        }


        public void SendMailAttachments(List<MailAddress> listacorreos, string subject, string body, List<Attachment> listaarchivos)
        {
            try
            {
                string str = ConfigurationManager.AppSettings["userfrom"];
                string str2 = ConfigurationManager.AppSettings["smtpserver"];
                SmtpClient client = new SmtpClient();
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(str)
                };
                foreach (MailAddress address in listacorreos)
                {
                    message.To.Add(address);
                }
                foreach (Attachment attachment in listaarchivos)
                {
                    message.Attachments.Add(attachment);
                }
                message.Subject = "Subject";
                message.IsBodyHtml = true;
                message.Body = "Body";
                client.Host = str2;
                client.Send(message);
            }
            catch (Exception)
            {
                throw;
            }
        }


        private static void DisposeAttachments(MailMessage message)
        {
            foreach (Attachment attachment in message.Attachments)
            {
                attachment.Dispose();
            }
            message.Attachments.Dispose();
            message = null;
        }

        public bool DeleteFile(string strfilepath)
        {
            FileInfo file = new FileInfo(strfilepath);
            if (file.Exists)
            {
                file.Delete();
            }
            return true;

        }
    }
}