namespace DiplomaServices.Services.Interfaces
{
    public interface IEmailVerificator
    {
        public void SendVerificationMessage(string emailToVerify);
        public string GenerateConfirmationToken();
        public void SetEmailAsVerified(int userId);
        public void SetConfirmationLink(string confirmationLink);
    }
}
