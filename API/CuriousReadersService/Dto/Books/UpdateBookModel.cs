namespace CuriousReadersData.Dto.Books;

using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class UpdateBookModel
{
    [Required]
    [MaxLength(128)]
    public string Title { get; set; }

    [Required]
    public string ISBN { get; set; }

    [MaxLength(1028)]
    public string? Description { get; set; }

    [Required]
    public string Status { get; set; }

    [Required]
    public int Quantity { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    public string OldImageUrl { get; set; }

    public IFormFile? Image { get; set; }

    [Required]
    public IEnumerable<string> Genres { get; set; } = new HashSet<string>();

    [Required]
    public IEnumerable<string> Authors { get; set; } = new HashSet<string>();
}
