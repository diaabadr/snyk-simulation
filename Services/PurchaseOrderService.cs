using Microsoft.Extensions.Logging;
using TestingSnyk.Models;

namespace TestingSnyk.Services;

public class PurchaseOrderService
{
    private readonly ILogger<PurchaseOrderService> _logger;

    public PurchaseOrderService(ILogger<PurchaseOrderService> logger)
    {
        _logger = logger;
    }

    public IResult ProcessPurchaseOrder(string action, PurchaseOrderRequest request)
    {
        try
        {
            // Simulate processing that may fail
            if (string.IsNullOrEmpty(request.VendorPo))
                throw new ArgumentException("VendorPo is required");

            // Simulate success
            _logger.LogInformation("PO {Action} succeeded | Store: {StoreId}, VendorPo: {VendorPo}, Vendor: {VendorId}, User: {UserId}",
                action, request.StoreId, request.VendorPo, request.VendorId, request.UserId);

            return Results.Ok(new { Status = "Success", Action = action });
        }
        catch (Exception ex)
        {
            // This is the pattern that triggers Snyk log injection alert
            _logger.LogError("PO {Action} failed | Store: {StoreId}, VendorPo: {VendorPo}, Vendor: {VendorId}, User: {UserId}, Error: {Error}",
                action, request.StoreId, request.VendorPo, request.VendorId, request.UserId, ex.Message);

            return Results.BadRequest(new { Status = "Failed", Error = ex.Message });
        }
    }
}
