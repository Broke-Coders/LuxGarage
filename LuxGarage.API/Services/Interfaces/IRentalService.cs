using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;

namespace LuxGarage.API.Services.Interfaces;

/// <summary>
/// Defines the core business logic for vehicle rentals in the LuxGarage system.
/// This service handles availability validation, pricing calculations, and the 
/// execution of the rental booking process.
/// </summary>
public interface IRentalService
{
    /// <summary>
    /// Checks if a specific vehicle is available for rental within a given time period.
    /// A vehicle is considered unavailable if it has overlapping active rentals or is in maintenance.
    /// </summary>
    /// <param name="vehicleId">The unique identifier of the vehicle.</param>
    /// <param name="start">The intended start date and time of the rental.</param>
    /// <param name="end">The intended end date and time of the rental.</param>
    /// <returns>True if the vehicle is available; otherwise, false.</returns>
    Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime start, DateTime end);

    /// <summary>
    /// Processes and creates a new rental transaction, managing customer association and pricing.
    /// </summary>
    /// <param name="request">The rental creation request containing vehicle, customer, and date details.</param>
    /// <param name="employeeId">The ID of the employee processing the rental.</param>
    /// <returns>A response DTO representing the finalized rental details.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the vehicle is not available for the requested period.</exception>
    Task<RentalResponse> CreateRentalAsync(CreateRentalRequest request, int employeeId);

    /// <summary>
    /// Calculates the total estimated price for a rental based on the vehicle's daily rate and duration.
    /// Includes logic for long-term rental discounts (e.g., periods over 30 days).
    /// </summary>
    /// <param name="vehicleId">The unique identifier of the vehicle.</param>
    /// <param name="start">The start date of the rental.</param>
    /// <param name="end">The end date of the rental.</param>
    /// <returns>The total calculated price for the rental period.</returns>
    Task<decimal> CalculateTotalPriceAsync(int vehicleId, DateTime start, DateTime end);
}