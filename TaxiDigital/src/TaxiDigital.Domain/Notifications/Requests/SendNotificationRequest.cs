namespace TaxiDigital.Domain.Notifications.Requests;

public sealed class SendNotificationRequest
{
    public string UserID { get; set; }
    public int RideStatusID { get; set; }
    public string Provider { get; set; }
    public string Vehicle { get; set; }
    public string Driver { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public int RideID { get; set; }
    public int ProviderID { get; set; }
    public string Date { get; set; }
    public string Plate { get; set; }
    public string WaitingTime { get; set; }
    public string DriverPhone { get; set; }
    public string Price { get; set; }
    public string Updated { get; set; }
}
