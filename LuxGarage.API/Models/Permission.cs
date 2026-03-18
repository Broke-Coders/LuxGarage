using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxGarage.API.Models;

public class Permission
{
    public int Id { get; set; }

    public string? Name {get; set;}

    public ICollection<Worker> Workers {get;} = new List<Worker>();
}
