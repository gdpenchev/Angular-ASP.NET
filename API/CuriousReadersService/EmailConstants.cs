namespace CuriousReadersService;

using System.Text;
using static CuriousReadersData.DataConstants;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class EmailConstants
{
    public const string subjectReminder = "Book Reminder";

    public const string subjectApproved = "Book Approval";

    public const string subjectUserApproval = "User Approved";

    public const string subjectPasswordRecovery = "Password Recovery";

    public const string subjectRejectBookReservation = "Book Reservation Rejected";

    public const string subjectRejectedProlonging = "Reservation Prolonging Rejected";

    public static string ContentForReservationApproval(string title, string isbn, string firstName)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"<h1> Hello, {firstName}! I am pleased to notify you that your book {title} with ISBN {isbn} request has been approved.<h1>");
        sb.AppendLine($"<p>We invite you to come into our library and get your book. For more information on where you can get your book, please visit Our Contacts page on our CuriousReader <a href={WebsiteBaseUrl}>Website</а></p>");
        return sb.ToString();
    }

    public static string ContentForUserApproval(string firstName)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"<h1> Hello, {firstName}! Your account has been approved</h1>");
        sb.AppendLine($"<p>To log into your account, please visit our CuriousReader <a href={WebsiteBaseUrl}> Website</а></p>");
        return sb.ToString();
    }
    public static string ContentForBookReminder(string firstName)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"<h1> Hello, {firstName}! Please note that some books need to be returned soon.</h1>");
        sb.AppendLine($"<p>For more information, please vising the Notifications page on our CuriousReader <a href={WebsiteBaseUrl}> Website</а></p>");

        return sb.ToString();
    }
    public static string ContentForRejecedProlongation(string firstName, string bookName)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"<h1> Hello, {firstName}! We are sorry to inform you that your request for prolonging the borrowed book {bookName} has been rejected.</h1>");
        sb.AppendLine($"<p> We invite you to checkout some of our other books on our CuriousReader <a href={WebsiteBaseUrl}> Website</а></p>");

        return sb.ToString();
    }

    public static string ContentForPasswordRecovery(string firstName, string tokenUrl)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"<h1> Hello, {firstName}! Forgot password request has been sent to this email.</h1>");
        sb.AppendLine($"<p>To recover you password, please click on the following link: <a href={tokenUrl}>Password recovery</а></p>");


        return sb.ToString();
    }

    public static string ContentForBookReservationRejection(string firstName)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"<h1> Hello, {firstName}! We regret to inform you that the book your requested is unavailable at this time.</h1>");

        return sb.ToString();
    }
}
