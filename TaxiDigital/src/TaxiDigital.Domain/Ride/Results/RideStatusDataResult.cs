namespace TaxiDigital.Domain.Ride.Results;

public sealed class RideStatusDataResult
{
    public string in_progress { get; set; }
    public string status_id { get; set; }
    public string status_desc { get; set; }
    public string phone_number { get; set; }
    public string init_lat { get; set; }
    public string init_lng { get; set; }
    public string end_lat { get; set; }
    public string end_lng { get; set; }
    public string driver_name { get; set; }
    public string driver_photo { get; set; }
    public string driver_prefix { get; set; }
    public string informed_time { get; set; }
    public string passed_informed_time { get; set; }
    public string license_plate { get; set; }
    public string car_brand { get; set; }
    public string car_model { get; set; }
}
