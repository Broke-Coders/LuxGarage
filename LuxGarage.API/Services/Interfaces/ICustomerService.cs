using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;

namespace LuxGarage.API.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for a service that manages customer data within the LuxGarage API.
    /// Provides methods for comprehensive customer profile management, including retrieval, 
    /// creation, updates, and deletion of customer records.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Retrieves all customers currently registered in the system.
        /// </summary>
        /// <returns>A collection of <see cref="CustomerResponse"/> objects.</returns>
        Task<IEnumerable<CustomerResponse>> GetAllAsync();

        /// <summary>
        /// Retrieves detailed information for a specific customer by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <returns>A <see cref="CustomerResponse"/> if found; otherwise, null.</returns>
        Task<CustomerResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new customer record based on the provided request data.
        /// </summary>
        /// <param name="request">The data for the new customer.</param>
        /// <returns>The created <see cref="CustomerResponse"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a customer with the same email already exists.</exception>
        Task<CustomerResponse> CreateAsync(CreateCustomerRequest request);

        /// <summary>
        /// Updates an existing customer's profile information.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to update.</param>
        /// <param name="request">The updated customer data.</param>
        /// <returns>The updated <see cref="CustomerResponse"/> if successful; otherwise, null.</returns>
        Task<CustomerResponse?> UpdateAsync(int id, UpdateCustomerRequest request);

        /// <summary>
        /// Removes a customer record from the system based on their identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
