using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxGarage.API.Models;
public class VehicleStatus
{
    public int Id { get; set; }

    public string Description { get; set; } = "UNKNOWN";

    public required DateTime StartingDate { get; set; }
    public DateTime? DateToEnd { get; set; }
}