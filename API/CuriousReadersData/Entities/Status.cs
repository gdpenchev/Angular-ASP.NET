namespace CuriousReadersData.Entities;

using System.ComponentModel.DataAnnotations;

public class Status
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual IEnumerable<Book> Books { get; set; } = new HashSet<Book>();
}
