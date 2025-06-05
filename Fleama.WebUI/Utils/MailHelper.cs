using Fleama.Core.Entities;
using System.Net;
using System.Net.Mail;

namespace Fleama.WebUI.Utils
{
    public class MailHelper
    {
        public static async Task SendMailAsync(Contact contact)
        {
            SmtpClient smtpClient = new SmtpClient("mail.siteadi.com", 587);
            smtpClient.Credentials = new NetworkCredential("info@siteadi.com", "mailşifresi");
            smtpClient.EnableSsl = false;
            MailMessage message = new MailMessage();
            message.From = new MailAddress("mail.siteadi.com");
            message.To.Add("mailnereyegönderilcek");
            message.Subject = "Siteden mail geldi";
            message.Body = "";
            message.IsBodyHtml = true;
            await smtpClient.SendMailAsync(message);
            smtpClient.Dispose();
            
        }
    }
}
