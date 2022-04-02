using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersData.Dto.Comments;

[ExcludeFromCodeCoverage]
public class ReadCommentModel
{
    public int Id { get; set; }

    public string Content { get; set; }

    public string UserName { get; set; }

    public int Rating { get; set; }

    public string BookName { get; set; }

    public string CreationDate { get; set; }

    public string CreationTime { get; set; }
}
