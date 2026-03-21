using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class WorkerRepository : IWorkerRepository
{
    private readonly RentalContext _context;

    public WorkerRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<Worker?> GetByIdAsync(int id) 
        => await _context.Workers.FindAsync(id);
    
    public async Task AddAsync(Worker worker)
    {
        await _context.Workers.AddAsync(worker);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Worker worker, int id)
    {
        Worker? oldWorker = await _context.Workers.FindAsync(id);

        oldWorker = worker;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var worker = await _context.Workers.FindAsync(id);
        
        if (worker == null)
        {
            Console.WriteLine("worker with given id not found");
            return;
        }

        _context.Workers.Remove(worker);
        await _context.SaveChangesAsync();
    }
}