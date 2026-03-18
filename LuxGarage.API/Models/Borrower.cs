using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxGarage.API.Models;
public class Borrower
{
    public int Id { get; set; }

    public required string Email { get; set; }

    public int BorrowCounter { get; set; }

    public ICollection<Rental> Rentals { get; } = new List<Rental>();
}