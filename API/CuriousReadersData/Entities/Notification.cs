namespace CuriousReadersData.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Notification
{
    [Key]
    public int Id { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedOn { get; set; }

    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }

    public virtual Book Book { get; set; }

    [ForeignKey(nameof(User))]
    public string UserId { get; set; }

    public virtual User User { get; set; }
}
