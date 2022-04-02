namespace CuriousReadersService.Services.Mail
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using CuriousReadersData.Entities;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using static CuriousReadersData.DataConstants;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class MailService : IMailService
    {
        private IConfiguration configuration;
        private readonly UserManager<User> userManager;

        public MailService(IConfiguration configuration, UserManager<User> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }
        public async Task SendBookReminderEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var subject = EmailConstants.subjectReminder;
            var htmlContent = EmailConstants.ContentForBookReminder(user.FirstName);
            await SendEmail(email, subject, htmlContent);

        }
        public async Task SendReservationApprovalEmail(string email, string title, string isbn, string firstName)
        {
            var subject = EmailConstants.subjectApproved;
            var htmlContent = EmailConstants.ContentForReservationApproval(title, isbn, firstName);
            await SendEmail(email, subject, htmlContent);
        }
        public async Task SendUserApprovalEmail(string email, string firstName)
        {
            var subject = EmailConstants.subjectUserApproval;
            var htmlContent = EmailConstants.ContentForUserApproval(firstName);
            await SendEmail(email, subject, htmlContent);
        }

        public async Task SendReservationRejectionMail(string email, string firstName)
        {
            var subject = EmailConstants.subjectRejectBookReservation;
            var htmlContent = EmailConstants.ContentForBookReservationRejection(firstName);
            await SendEmail(email, subject, htmlContent);
        }
        public async Task SendForgotPasswordEmail(string firstName, string email, string tokenUrl)
        {
            var subject = EmailConstants.subjectPasswordRecovery;
            var htmlContent = EmailConstants.ContentForPasswordRecovery(firstName, tokenUrl);
            await SendEmail(email, subject, htmlContent);
        }
        public async Task SendRejectProlongationEmail(string email, string firstName, string bookName)
        {
            var subject = EmailConstants.subjectRejectedProlonging;
            var htmlContent = EmailConstants.ContentForRejecedProlongation(firstName, bookName);
            await SendEmail(email, subject, htmlContent);
        }
        public async Task SendEmail(string email, string subject, string htmlContent)
        {
            var apiKey = configuration["SendGridApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(SenderEmailAddress, Librarian);
            var to = new EmailAddress(email, Reader);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}