namespace TaxiDigital.Domain.Ride.Results;

public sealed class DataRideInfoResult
{

    public string driver_name { get; set; }
    public string driver_photo { get; set; }
    public string license_plate { get; set; }
    public string informed_time { get; set; }
    public string car_brand { get; set; }
    public string car_model { get; set; }
}