using System.Net;
using System.Net.Mail;

namespace RetroFootballAPI.StaticServices
{
    public class Gmail
    {
        public IConfiguration _config;
        private static string _email = "";
        private static string _password = "";
        public Gmail(IConfiguration config)
        {
            _config = config;
            _email = _config["Gmail:Email"];
            _password = _config["Gmail:Password"];
        }

        public static bool SendEmail(string subject, string content, List<string> toMail)
        {
            try
            {
                var message = new MailMessage();
                var smtp = new SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential()
                    {
                        UserName = _email,
                        Password = _password
                    };
                }
                var fromAddress = new MailAddress(_email, "HVPP Sports");
                message.From = fromAddress;

                foreach (var mail in toMail)
                {
                    message.To.Add(mail);
                }

                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;
                smtp.Send(message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }
}
