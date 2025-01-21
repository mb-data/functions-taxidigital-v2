namespace TaxiDigital.Domain.Ride.Results;

public sealed class DataRideDetailsResult
{
    public string init_address { get; set; }
    public string init_reference { get; set; }
    public string init_lat { get; set; }
    public string init_lng { get; set; }
    public string end_lat { get; set; }
    public string end_lng { get; set; }
    public string end_address { get; set; }
    public string driver_name { get; set; }
    public string driver_photo { get; set; }
    public string driver_prefix { get; set; }
    public string license_plate { get; set; }
    public string car_model { get; set; }
    public string car_brand { get; set; }
    public string date { get; set; }
    public string passenger_name { get; set; }
    public string passenger_number { get; set; }
    public string payment_method { get; set; }
    public string ride_value { get; set; }
    public string init_date { get; set; }
    public string init_end { get; set; }
    public int time_in_seconds { get; set; }
    public string km { get; set; }
    public string status_id { get; set; }
    public string status_desc { get; set; }
    public string service_value { get; set; }
    public string stopped_time_value { get; set; }
    public string tollgate_fare { get; set; }
    public string parking_fare { get; set; }
    public string discount_value { get; set; }
}
