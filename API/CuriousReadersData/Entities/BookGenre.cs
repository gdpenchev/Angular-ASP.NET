namespace CuriousReadersData.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class BookGenre
{
    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }

    public virtual Book Book { get; set; }


    [ForeignKey(nameof(Genre))]
    public int GenreId { get; set; }

    public virtual Genre Genre { get; set; }
}
