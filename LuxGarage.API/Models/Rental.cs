using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxGarage.API.Models;
public class Rental
{
    public int Id { get; set; }

    public int VehicleId { get; set; }

    public Vehicle Vehicle {get; set;} = null!;

    public DateTime StartingTime { get; set; }

    public DateTime AppointedReturnTime { get; set; }

    public DateTime? RealReturnTime { get; set; }

    public int BorrowerId { get; set; }

    public Borrower Borrower { get; set; } = null!;

    public ICollection<RentalInsurance> RentalInsurances { get; } = new List<RentalInsurance>();
}