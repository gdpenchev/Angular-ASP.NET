using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersData.Dto.Comments;

[ExcludeFromCodeCoverage]
public class CreateCommentModel
{
    public int BookId { get; set; }

    public string UserName { get; set; }

    public string Content { get; set; }

    public int Rating { get; set; }
}
