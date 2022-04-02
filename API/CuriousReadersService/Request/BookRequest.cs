using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersService.Request;

[ExcludeFromCodeCoverage]
public class BookRequest
{
    public string Title { get; set; }

    public DateTime CreatedOn { get; set; }

    public string Description { get; set; }

    public double? Rating { get; set; }

    public int Quantity { get; set; }

    public string Genre { get; set; }

    public virtual IEnumerable<string> AuthorNames { get; set; } = new HashSet<string>();
}