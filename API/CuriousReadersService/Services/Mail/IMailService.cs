namespace CuriousReadersService.Services.Mail
{
    public interface IMailService
    {
        Task SendBookReminderEmail(string email);

        Task SendReservationApprovalEmail(string email,string title, string isbn,string firstName);

        Task SendUserApprovalEmail(string email, string firstName);

        Task SendReservationRejectionMail(string email, string firstName);

        Task SendForgotPasswordEmail(string email, string firstName, string tokenUrl);

        Task SendRejectProlongationEmail(string email, string firstName, string bookName);

        Task SendEmail(string email, string subject, string htmlContent);
    }
}
