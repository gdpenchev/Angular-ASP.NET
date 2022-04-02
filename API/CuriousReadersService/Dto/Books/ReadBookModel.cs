using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersData.Dto.Books;

[ExcludeFromCodeCoverage]
public class ReadBookModel
{
    public int Id { get; init; }
    
    public string ISBN { get; set; }

    public string Title { get; set; }

    public string Status { get; set; }

    public string CreatedOn { get; set; }

    public string ModifiedOn { get; set; }

    public string Description { get; set; }

    public double? Rating { get; set; }

    public int Quantity { get; set; }

    public string Image { get; set; }

    public IEnumerable<string> Genres { get; set; } = new HashSet<string>();

    public IEnumerable<string> Authors { get; set; } = new HashSet<string>();

    public IEnumerable<string> ReserveeEmails { get; set; } = new HashSet<string>();
}
