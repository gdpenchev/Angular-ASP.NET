namespace CuriousReadersData.Entities;

using System.ComponentModel.DataAnnotations;

public class Author
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public virtual IEnumerable<AuthorBook> Books { get; set; } = new HashSet<AuthorBook>();
}
