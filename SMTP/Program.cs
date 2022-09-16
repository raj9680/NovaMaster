using System;
using System.Net;
using System.Net.Mail;

namespace SMTP
{
    class Program
    {
        static void Main(string[] args)
        {
            string fromMail = "rohanarora98550@gmail.com";
            string fromPassword = "okeiqqwevgwnheuf";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Test Subject";
            message.To.Add(new MailAddress("rishabkqushal@gmail.com"));
            message.Body = "<html><body>This is test body</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }
    }
}
