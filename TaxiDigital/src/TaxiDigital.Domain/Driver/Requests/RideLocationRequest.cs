using System.ComponentModel.DataAnnotations;

namespace TaxiDigital.Domain.Driver.Requests;

public sealed class RideLocationRequest
{
    [Required]
    public string Latitude { get; set; }

    [Required]
    public string Longitude { get; set; }

    public int? Bearing { get; set; }

    public int? Eta { get; set; }
}
