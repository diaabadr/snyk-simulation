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
        _logger.LogInformation("PO {Action} received | Store: {StoreId}, User: {UserId}",
            action, request.StoreId, request.UserId);

        try
        {
            if (string.IsNullOrEmpty(request.VendorPo))
                throw new ArgumentException("VendorPo is required");

            _logger.LogInformation("PO {Action} succeeded | Store: {StoreId}, VendorPo: {VendorPo}, Vendor: {VendorId}, User: {UserId}",
                action, request.StoreId, request.VendorPo, request.VendorId, request.UserId);

            return Results.Ok(new { Status = "Success", Action = action });
        }
        

        catch (Exception ex)
        {
            _logger.LogError("PO {Action} failed | Store: {StoreId}, VendorPo: {VendorPo}, Vendor: {VendorId}, User: {UserId}, Error: {Error}",
                action, request.StoreId, request.VendorPo, request.VendorId, request.UserId, ex.Message);

            _logger.LogWarning("PO {Action} retry scheduled | Store: {StoreId}, VendorPo: {VendorPo}, Attempt: {Attempt}",
                action, request.StoreId, request.VendorPo, 1);

            return Results.BadRequest(new { Status = "Failed", Error = ex.Message });
        }
    }

    public IResult ValidateVendor(string vendorId, string userId)
    {
        _logger.LogInformation("Vendor validation started | Vendor: {VendorId}, RequestedBy: {UserId}",
            vendorId, userId);

        if (string.IsNullOrEmpty(vendorId))
        {
            _logger.LogWarning("Vendor validation failed | Vendor: {VendorId}, Reason: {Reason}",
                vendorId, "Empty vendor ID provided");
            return Results.BadRequest("Invalid vendor");
        }

        return Results.Ok(new { VendorId = vendorId, Valid = true });
    }
}
