namespace CuriousReadersData;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CuriousReadersData.Entities;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class LibraryDbContext : IdentityDbContext<User>
{
    public LibraryDbContext()
    {

    }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {

    }

    public virtual DbSet<Author> Authors { get; init; }

    public virtual DbSet<AuthorBook> AuthorsBooks { get; init; }

    public virtual DbSet<Book> Books { get; init; }

    public virtual DbSet<BookGenre> BooksGenres { get; init; }

    public virtual DbSet<Comment> Comments { get; init; }

    public virtual DbSet<Genre> Genres { get; init; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Reservation> Reservations { get; init; }

    public virtual DbSet<Notification> Notifications { get; init; }

    public override DbSet<User> Users { get => base.Users; set => base.Users = value; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AuthorBook>(e =>
        {
            e.HasKey(e => new { e.AuthorId, e.BookId });

            e.HasOne(ab => ab.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(ab => ab.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(ab => ab.Book)
            .WithMany(b => b.Authors)
            .HasForeignKey(ab => ab.BookId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Author>(e =>
        {
            e.HasIndex(u => u.Name)
            .IsUnique();
        });


        modelBuilder.Entity<Book>(b =>
        {
            b.HasKey(b => b.Id);

            b.HasIndex(b => b.ISBN)
            .IsUnique();

            b.Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(128);

            b.Property(b => b.Description)
                .IsRequired()
                .HasMaxLength(1028);

            b.HasOne(b => b.Status)
            .WithMany(s => s.Books)
            .HasForeignKey(b => b.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
            b.HasMany(b => b.Reservations)
            .WithOne(b => b.Book);
        });


        modelBuilder.Entity<BookGenre>(e =>
        {
            e.HasKey(e => new { e.BookId, e.GenreId });

            e.HasOne(bg => bg.Book)
            .WithMany(a => a.Genres)
            .HasForeignKey(ab => ab.BookId)
            .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(bg => bg.Genre)
            .WithMany(b => b.Books)
            .HasForeignKey(ab => ab.GenreId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Comment>(e =>
        {
            e.HasOne(e => e.Book)
            .WithMany(e => e.Comments)
            .HasForeignKey(e => e.BookId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Notification>(e =>
        {
            e.HasOne(e => e.Book)
            .WithMany(e => e.Notifications)
            .HasForeignKey(e => e.BookId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>()
        .Property(e => e.Id)
        .ValueGeneratedOnAdd();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
