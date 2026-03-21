
using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IBorrowerRepository
{
    Task<Borrower?> GetByIdAsync(int id);
    Task AddAsync(Borrower borrower);
    Task UpdateAsync(Borrower borrower);
    Task DeleteAsync(int id);
}