using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages rental insurance data in the LuxGarage API, 
/// providing methods for retrieving, adding, updating, and deleting rental insurance information from the database.
/// </summary>
public interface IRentalInsuranceRepository
{
    Task<RentalInsurance?> GetByIdAsync(int id);
    Task AddAsync(RentalInsurance rentalInsurance);
    Task UpdateAsync(RentalInsurance rentalInsurance, int id);
    Task DeleteAsync(int id);
}