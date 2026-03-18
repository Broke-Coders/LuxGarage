using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Insurance
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public ICollection<RentalInsurance> RentalInsurances { get; set; } = new List<RentalInsurance>();
}