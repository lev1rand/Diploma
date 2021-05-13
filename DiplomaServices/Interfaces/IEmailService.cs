namespace DiplomaServices.Interfaces
{
    public interface IEmailService
    {
        public void SendMessage(string applicantEmail, string message, string confirmationLink);
        public string GenerateConfirmationToken();
        public void SetEmailAsVerified(int userId);
        public void SetConfirmationLink(string confirmationLink);
    }
}
