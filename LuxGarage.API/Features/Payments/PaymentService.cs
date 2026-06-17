using LuxGarage.API.Data;
using LuxGarage.API.Features.Rentals;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Features.Payments;

public class PaymentService
{
    private readonly RentalContext _context;

    public PaymentService(RentalContext context)
    {
        _context = context;
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(ProcessPaymentRequest request)
    {
        var rental = await _context.Rentals
            .FirstOrDefaultAsync(r => r.Id == request.RentalId);

        if (rental == null)
            return new PaymentResponse { Success = false, Message = "Rental not found." };

        if (rental.Status != RentalStatus.ReservedWaitingForPayment)
            return new PaymentResponse { Success = false, Message = "Rental is not in a state awaiting payment." };

        // Simulate payment processing delay
        await Task.Delay(1000);

        // Update rental status to Pending (Paid, awaiting rental period)
        rental.Status = RentalStatus.Pending;
        await _context.SaveChangesAsync();

        return new PaymentResponse
        {
            Success = true,
            TransactionId = Guid.NewGuid().ToString(),
            Message = "Payment successful and rental confirmed."
        };
    }
}
