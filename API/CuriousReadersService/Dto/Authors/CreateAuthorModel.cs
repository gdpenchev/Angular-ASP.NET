namespace CuriousReadersData.Dto.Authors;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class CreateAuthorModel
{
    [Required]
    public IEnumerable<string> Authors { get; set; } = new HashSet<string>();

}
