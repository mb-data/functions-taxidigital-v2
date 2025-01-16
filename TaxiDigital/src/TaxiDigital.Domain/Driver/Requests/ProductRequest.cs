namespace TaxiDigital.Domain.Driver.Requests;

public sealed class ProductRequest
{
    public ProductRequest(string productId, int providerId, int categoryId, string description)
    {
        ProductID = productId;
        ProviderID = providerId;
        CategoryID = categoryId;
        Description = description;
    }

    public string ProductID { get; set; }

    public int ProviderID { get; set; }

    public int CategoryID { get; set; }

    public string Description { get; set; }
}
