using CuriousReadersData.Dto.Books;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersData.Dto.Reservations;

[ExcludeFromCodeCoverage]
public class ReadReservationModel
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public ReadBookModel Book { get; set; }

    public string Status { get; set; }

    public DateTime? RequestDate { get; set; }

    public DateTime? ReturnDate { get; set; }
}
