using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersService.Request;

[ExcludeFromCodeCoverage]
public class ReservationRequest
{
    public int BookId { get; set; }

    public string UserEmail { get; set; }
}