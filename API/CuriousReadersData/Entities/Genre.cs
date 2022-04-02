namespace CuriousReadersData.Entities;

using System.ComponentModel.DataAnnotations;

public class Genre
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public virtual IEnumerable<BookGenre> Books { get; set; } = new HashSet<BookGenre>();
}
