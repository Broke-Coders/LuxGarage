namespace LuxGarage.API.Features.Payments;

public class ProcessPaymentRequest
{
    public int RentalId { get; set; }
    public string PaymentMethod { get; set; } = "MockCard";
    public string Currency { get; set; } = "PLN";
}

public class PaymentResponse
{
    public bool Success { get; set; }
    public string TransactionId { get; set; } = null!;
    public string Message { get; set; } = null!;
}
