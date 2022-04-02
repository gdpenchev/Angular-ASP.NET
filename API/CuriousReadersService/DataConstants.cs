namespace CuriousReadersData;

public class DataConstants
{
    public const string Reader = "Reader";

    public const string Librarian = "Librarian";

    public const string SenderEmailAddress = "librarian_b65-490d-b088-e3a6d9@abv.bg";

    public const string WebsiteBaseUrl = "https://schoolofdotnet2022-pirin.azurewebsites.net/";

    public const string phoneNumberRegex = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";

    public const string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[\W])(?=^.{10,65}$)";

    public const string isbnRegex = @"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$";
}
