using System.Net;
using System.Net.Mail;
using System.Text;

namespace Email
{
    public class EmailVerification
    {
        public static string Main(string email, int userId)
        {
            string fromMail = "rohanarora98550@gmail.com";
            string fromPassword = "okeiqqwevgwnheuf";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Reset your password | Nova Immigrations";
            message.To.Add(new MailAddress(email));
            var domain = "http://localhost:5000";

            var sb = new StringBuilder("<body style='margin: 0px;'>");
            sb.AppendFormat("<div><a href='{0}/common/resetpassword?anonymous={1}&get=%45634&encoded&user=rebecca'>Click here </a> to reset your password</div>", domain, userId);
            sb.Append("</body>");

            //message.Body = $"<html><body>Reset your password <a href='http://localhost:5000/common/resetpassword?anonymous='+'{userId}'>here</a></body></html>";
            message.Body = sb.ToString();
            message.IsBodyHtml = true;
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true
                };

                smtpClient.Send(message);
                return "success";
            }
            catch (System.Exception)
            {
                return "failed";
            }
            
        }
    }
}
