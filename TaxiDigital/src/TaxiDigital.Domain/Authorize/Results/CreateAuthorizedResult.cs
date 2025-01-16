namespace TaxiDigital.Domain.Authorize.Results;

public sealed class CreateAuthorizedResult
{
    public int status { get; set; }
    public string message { get; set; }
    public string authorized_id { get; set; }
}
