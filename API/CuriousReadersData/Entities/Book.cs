namespace CuriousReadersData.Entities;

using System.ComponentModel.DataAnnotations;

public class Book
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string ISBN { get; set; }

    [Required]
    [MaxLength(128)]
    public string Title { get; set; }

    public int StatusId { get; set; }

    public virtual Status Status { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    [MaxLength(1028)]
    public string Description { get; set; }

    public int Quantity { get; set; }

    public string Image { get; set; }

    public virtual IEnumerable<BookGenre> Genres { get; set; } = new HashSet<BookGenre>();

    public virtual IEnumerable<Comment> Comments { get; set; } = new HashSet<Comment>();

    public virtual IEnumerable<AuthorBook> Authors { get; set; } = new HashSet<AuthorBook>();

    public virtual IEnumerable<Reservation> Reservations { get; set; } = new HashSet<Reservation>();

    public virtual IEnumerable<Notification> Notifications { get; set; } = new HashSet<Notification>();

}
