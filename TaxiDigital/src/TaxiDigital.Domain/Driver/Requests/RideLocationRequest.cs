using System.ComponentModel.DataAnnotations;

namespace TaxiDigital.Domain.Driver.Requests;

public sealed class RideLocationRequest
{
    [Required]
    public string Lat { get; set; }

    [Required]
    public string Lng { get; set; }

    public int? Bearing { get; set; }

    public int? Eta { get; set; }
}
