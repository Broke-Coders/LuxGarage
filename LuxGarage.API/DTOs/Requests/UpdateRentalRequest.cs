using LuxGarage.API.Models;

namespace LuxGarage.API.DTOs.Requests;

/// <summary>
/// Data transfer object for updating an existing rental.
/// </summary>
public class UpdateRentalRequest
{
    /// <summary>Gets or sets the updated scheduled return date and time.</summary>
    public DateTime? AppointedReturnTime { get; set; }
    /// <summary>Gets or sets the updated status of the rental.</summary>
    public RentalStatus? Status { get; set; }
}