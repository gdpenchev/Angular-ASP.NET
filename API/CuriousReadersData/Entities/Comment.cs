namespace CuriousReadersData.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Comment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Content { get; set; }

    public int Rating { get; set; } 

    public bool IsAproved { get; set; }

    public string Username { get; set; }

    public DateTime CreatedOn { get; set; }


    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }

    public virtual Book Book { get; set; }
}
