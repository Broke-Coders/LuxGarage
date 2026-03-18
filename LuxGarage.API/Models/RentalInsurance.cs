using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class RentalInsurance
{
    [Key]
    public int Id { get; set; }

    public int RentalId { get; set; }

    public int InsuranceId { get; set; }

    [ForeignKey(nameof(RentalId))]
    public Rental Rental { get; set; }

    [ForeignKey(nameof(InsuranceId))]
    public Insurance Insurance { get; set; }
}