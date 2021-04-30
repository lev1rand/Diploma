using DataAccess;
using System;
using System.Net;
using System.Net.Mail;

namespace TestTaskServices.Services.Interfaces
{
    public class EmailVerificator : IEmailVerificator
    {
        #region private fields

        private const string SENDER_EMAIL = "testonsender@gmail.com";

        private const string SENDER_PASSWORD = "helloit'sme";

        private readonly IUnitOfWork uow;

        private string confirmationLink;

        #endregion

    public EmailVerificator(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public void SendVerificationMessage(string emailToVerify)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(SENDER_EMAIL, SENDER_PASSWORD);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            var message = CreateMessage(emailToVerify, confirmationLink);

            smtp.Send(message);
        }
        public void SetConfirmationLink(string confirmationLink)
        {
            this.confirmationLink = confirmationLink;
        }
        public void SetEmailAsVerified(int userId)
        {
            var user = uow.Users.Get(userId);
            user.IsEmailVerified = true;
            uow.Users.Update(user);

            uow.Save();
        }
        public string GenerateConfirmationToken()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
        private MailMessage CreateMessage(string emailToVerify, string confirmationLink)
        {
            MailMessage message = new MailMessage();

            message.From = new MailAddress(SENDER_EMAIL);
            message.To.Add(new MailAddress(emailToVerify));
            message.Subject = "Hello there! Confirm your email, please! ";
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = string.Format("Please, tab on this link {0} to verify your email and then go back to the website. " +
                "Don't share this link with third parties! Have a good day :)", confirmationLink);

            return message;
        }
    }
}
