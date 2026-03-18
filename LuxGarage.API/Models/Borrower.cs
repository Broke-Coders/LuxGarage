using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Borrower
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Email { get; set; }

    public int BorrowCounter { get; set; }

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}