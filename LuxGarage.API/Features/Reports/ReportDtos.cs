namespace LuxGarage.API.Features.Reports;

public enum ReportPeriod
{
    Daily = 1,
    Weekly = 2,
    Monthly = 3
}

public class OperationalReportRequest
{
    public ReportPeriod Period { get; set; } = ReportPeriod.Daily;
    public DateTime ReferenceDate { get; set; } = DateTime.UtcNow;
}

public class OperationalReportResponse
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalRentals { get; set; }
    public decimal TotalRevenue { get; set; }
    public double OccupancyRate { get; set; } // Percentage 0-100
    public int ActiveVehiclesCount { get; set; }
    public List<DailyReportMetric> DailyBreakdown { get; set; } = new();
}

public class DailyReportMetric
{
    public DateTime Date { get; set; }
    public int RentalsCount { get; set; }
    public decimal Revenue { get; set; }
    public double OccupancyRate { get; set; }
}
