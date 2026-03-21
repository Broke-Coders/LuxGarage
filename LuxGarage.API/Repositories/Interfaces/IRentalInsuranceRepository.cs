using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IRentalInsuranceRepository
{
    Task<RentalInsurance?> GetByIdAsync(int id);
    Task AddAsync(RentalInsurance rentalInsurance);
    Task UpdateAsync(RentalInsurance rentalInsurance, int id);
    Task DeleteAsync(int id);
}