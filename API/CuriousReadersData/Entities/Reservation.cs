namespace CuriousReadersData.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Reservation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public virtual Status Status { get; set; }

    public DateTime? RequestDate { get; set; }  

    public DateTime? ReturnDate { get; set; }


    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }

    public virtual Book Book { get; set; }

    [ForeignKey(nameof(User))]
    public string UserId { get; set; }

    public virtual User User { get; set; }
}
