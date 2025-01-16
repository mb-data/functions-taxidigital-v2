namespace TaxiDigital.Domain.Ride.Requests;

public sealed class UniqueFieldRequest
{
    public string unique_field { get; set; }
    public List<ClassificatorRequest> classificators { get; set; }
}
