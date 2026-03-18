using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxGarage.API.Models;

public class Worker
{
    public int Id { get; set; }

    public required string Password { get; set; }

    public int WorkplaceId { get; set; }
    public Workplace Workplace { get; set; } = null!;

    public int PermissionId { get; set; }

    public Permission Permission {get; set;} = null!;
}
