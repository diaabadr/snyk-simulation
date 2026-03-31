namespace TestingSnyk.Models;

public class PurchaseOrderRequest
{
    public string StoreId { get; set; } = string.Empty;
    public string VendorPo { get; set; } = string.Empty;
    public string VendorId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}
