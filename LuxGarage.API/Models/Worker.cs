using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Worker
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Password { get; set; }

    public int WorkplaceId { get; set; }

    public int PermissionId { get; set; }

    [ForeignKey(nameof(WorkplaceId))]
    public Workplace Workplace { get; set; }
}
