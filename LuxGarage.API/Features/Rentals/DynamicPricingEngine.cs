namespace LuxGarage.API.Features.Rentals;

public class RentalPricingContext
{
    public decimal BasePricePerDay { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalDays => (EndDate.Date - StartDate.Date).Days;
}

public interface IPricingStrategy
{
    decimal CalculateMultiplier(RentalPricingContext context);
}

public class LongTermDiscountStrategy : IPricingStrategy
{
    public decimal CalculateMultiplier(RentalPricingContext context)
    {
        return context.TotalDays > 30 ? 0.8m : 1.0m;
    }
}

public class WeekendSurchargeStrategy : IPricingStrategy
{
    public decimal CalculateMultiplier(RentalPricingContext context)
    {
        bool hasWeekend = false;
        for (var date = context.StartDate.Date; date <= context.EndDate.Date; date = date.AddDays(1))
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                hasWeekend = true;
                break;
            }
        }
        return hasWeekend ? 1.1m : 1.0m;
    }
}

public class DynamicPricingEngine
{
    private readonly IEnumerable<IPricingStrategy> _strategies;

    public DynamicPricingEngine(IEnumerable<IPricingStrategy> strategies)
    {
        _strategies = strategies;
    }

    public decimal CalculateTotal(RentalPricingContext context)
    {
        if (context.TotalDays < 1) 
            throw new ArgumentException("The rental period must be at least one day.");

        decimal currentDailyRate = context.BasePricePerDay;

        foreach (var strategy in _strategies)
        {
            currentDailyRate *= strategy.CalculateMultiplier(context);
        }

        return Math.Round(currentDailyRate * context.TotalDays, 2);
    }
}