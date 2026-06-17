using LuxGarage.API.Data;
using LuxGarage.API.Features.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Features.Reports;

public class ReportService
{
    private readonly RentalContext _context;

    public ReportService(RentalContext context)
    {
        _context = context;
    }

    public async Task<OperationalReportResponse> GetOperationalReportAsync(OperationalReportRequest request)
    {
        var (startDate, endDate) = CalculateDateRange(request.Period, request.ReferenceDate);

        var rentals = await _context.Rentals
            .Where(r => r.StartingTime < endDate && r.AppointedReturnTime > startDate)
            .ToListAsync();

        var activeVehiclesCount = await _context.Vehicles
            .CountAsync(v => v.Status != VehicleStatus.Retired);

        var response = new OperationalReportResponse
        {
            StartDate = startDate,
            EndDate = endDate,
            ActiveVehiclesCount = activeVehiclesCount,
            TotalRentals = rentals.Count(r => r.StartingTime >= startDate && r.StartingTime < endDate),
            TotalRevenue = rentals
                .Where(r => r.StartingTime >= startDate && r.StartingTime < endDate)
                .Sum(r => r.TotalPrice)
        };

        // Calculate occupancy and daily breakdown
        for (var date = startDate.Date; date < endDate.Date; date = date.AddDays(1))
        {
            var nextDay = date.AddDays(1);
            
            // A rental contributes to occupancy if it overlaps with this specific day
            var dayRentals = rentals.Where(r => r.StartingTime < nextDay && r.AppointedReturnTime > date).ToList();
            
            var dailyRevenue = dayRentals
                .Where(r => r.StartingTime >= date && r.StartingTime < nextDay)
                .Sum(r => r.TotalPrice);

            double dailyOccupancy = activeVehiclesCount > 0 
                ? (double)dayRentals.Count / activeVehiclesCount * 100 
                : 0;

            response.DailyBreakdown.Add(new DailyReportMetric
            {
                Date = date,
                RentalsCount = dayRentals.Count(r => r.StartingTime >= date && r.StartingTime < nextDay),
                Revenue = dailyRevenue,
                OccupancyRate = Math.Round(dailyOccupancy, 2)
            });
        }

        if (response.DailyBreakdown.Any())
        {
            response.OccupancyRate = Math.Round(response.DailyBreakdown.Average(d => d.OccupancyRate), 2);
        }

        return response;
    }

    private static (DateTime start, DateTime end) CalculateDateRange(ReportPeriod period, DateTime referenceDate)
    {
        DateTime start, end;
        var date = DateTime.SpecifyKind(referenceDate.Date, DateTimeKind.Utc);

        switch (period)
        {
            case ReportPeriod.Daily:
                start = date;
                end = date.AddDays(1);
                break;
            case ReportPeriod.Weekly:
                // Start of week (Monday)
                int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
                start = date.AddDays(-1 * diff);
                end = start.AddDays(7);
                break;
            case ReportPeriod.Monthly:
                start = new DateTime(date.Year, date.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                end = start.AddMonths(1);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(period));
        }

        return (start, end);
    }
}
