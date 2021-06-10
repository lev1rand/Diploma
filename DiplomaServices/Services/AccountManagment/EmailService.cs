using DataAccess;
using DiplomaServices.Interfaces;
using System;
using System.Net;
using System.Net.Mail;

namespace DiplomaServices.Services.AccountManagment
{
    public class EmailService : IEmailService
    {
        #region private fields

        private const string SENDER_EMAIL = "testonsender@gmail.com";

        private const string SENDER_PASSWORD = "helloit'sme";

        private readonly IUnitOfWork uow;

        private string confirmationLink;

        #endregion

        public EmailService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public void SendMessage(string applicantEmail, string message, string confirmationLink)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(SENDER_EMAIL, SENDER_PASSWORD);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            if (message == null)
            {
                var messageContent = CreateMessage(applicantEmail, null, confirmationLink);
                smtp.Send(messageContent);

                return;
            }
            else
            {
                var messageContent = CreateMessage(applicantEmail, message, null);
                smtp.Send(messageContent);

                return;
            }


        }
        public void SetConfirmationLink(string confirmationLink)
        {
            this.confirmationLink = confirmationLink;
        }
        public void SetEmailAsVerified(int userId)
        {
            var user = uow.Users.Get(u => u.Id == userId);
            if (user != null)
            {
                user.IsEmailVerified = true;

                uow.Users.Update(user);
                uow.Save();
            }
            else
            {
                throw new Exception(string.Format("Користувач з id {0} не знайдений!", userId));
            }
        }
        public string GenerateConfirmationToken()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        private MailMessage CreateMessage(string applicantEmail, string messageText, string confirmationLink)
        {
            MailMessage message = new MailMessage();

            message.From = new MailAddress(SENDER_EMAIL);
            message.To.Add(new MailAddress(applicantEmail));
            message.Subject = "You have one more message from TestOn portal! ";
            message.IsBodyHtml = true; //to make message body as html  

            if (messageText == null && confirmationLink != null)
            {
                message.Body = GetConfirmationMessageText(confirmationLink);
            }
            else
            {
                message.Body = messageText;
            }

            return message;
        }
        private string GetConfirmationMessageText(string confirmationLink)
        {
            return string.Format("Привіт! Будь ласка, натисни на цей лінк {0}, щоб підтвердити свою електронну пошту в системі. Після цього можеш повернутися назад до веб-сайту TestOnSystem. " +
                "Увага! Не розповсюджуй це посилання серед третіх осіб! Бажаємо гарного дня :)", confirmationLink);
        }
    }
}

