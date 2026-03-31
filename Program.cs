using TestingSnyk.Models;
using TestingSnyk.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<PurchaseOrderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapPost("/api/po/{action}", (string action, PurchaseOrderRequest request, PurchaseOrderService poService) =>
{
    return poService.ProcessPurchaseOrder(action, request);
});

app.MapGet("/api/vendor/{vendorId}/validate", (string vendorId, string userId, PurchaseOrderService poService) =>
{
    return poService.ValidateVendor(vendorId, userId);
});

app.MapDelete("/api/po/{orderId}", (string orderId, string userId, string reason, PurchaseOrderService poService) =>
{
    return poService.CancelPurchaseOrder(orderId, userId, reason);
});

app.Run();
