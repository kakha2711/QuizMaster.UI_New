
using System.Net.Mail;

namespace QuizMaster.Service
{
    public class EmailService
    {
        public static void SendEmail(string to, string subject, string body)
        {


            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("datunashvilid0@gmail.com", "hqqs xlbv yref jyae");



            MailMessage message = new MailMessage();
            message.From = new MailAddress("datunashvilid0@gmail.com");
            message.To.Add(to);
            //message.To.Add("stepacc210@gmail.com");
            message.Subject = subject;
            message.Body = body;

            


            smtp.Send(message);
        }
    }
}
