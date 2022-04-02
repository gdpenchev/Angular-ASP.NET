using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersAPI.Infrastructure;

[ExcludeFromCodeCoverage]
public class ApplicationSettings
{
    public string JWT_Secret { get; set; }
    public string Client_URL { get; set; }
}
