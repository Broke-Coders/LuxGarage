using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxGarage.API.Models;
public class RentalInsurance
{
    public int Id { get; set; }

    public int RentalId { get; set; }

    public Rental Rental { get; set; } = null!;
    public int InsuranceId { get; set; }
    public Insurance Insurance { get; set; } = null!;
}