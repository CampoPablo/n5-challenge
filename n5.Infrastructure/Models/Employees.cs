using System.ComponentModel.DataAnnotations;

namespace n5.Infrastructure.Models;

public class Employees
{
    [Key]
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Department { get; set; }
    public required string IdentificationDocument { get; set; }
}
