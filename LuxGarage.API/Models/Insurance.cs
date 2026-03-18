using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxGarage.API.Models;
public class Insurance
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required decimal Price { get; set; }

    public ICollection<RentalInsurance> RentalInsurances { get; } = new List<RentalInsurance>();
}