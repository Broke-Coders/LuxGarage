using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Rental
{
    [Key]
    public int Id { get; set; }

    public int CarId { get; set; }

    public DateTime StartingTime { get; set; }

    public DateTime AppointedReturnTime { get; set; }

    public DateTime? RealReturnTime { get; set; }

    public int BorrowerId { get; set; }

    [ForeignKey(nameof(BorrowerId))]
    public Borrower Borrower { get; set; }

    public ICollection<RentalInsurance> RentalInsurances { get; set; } = new List<RentalInsurance>();
}