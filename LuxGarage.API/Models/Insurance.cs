using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxGarage.API.Models;
/// <summary>
/// Represents an insurance option in the LuxGarage system, containing properties for the insurance's ID, name, price, and a collection of 
/// rental insurances associated with the insurance. This class serves as a data model for insurance options in the application, allowing for the
/// storage and retrieval of insurance information and their associations with rentals in the system.
/// </summary>
public class Insurance
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required decimal Price { get; set; }

    public ICollection<RentalInsurance> RentalInsurances { get; } = new List<RentalInsurance>();
}