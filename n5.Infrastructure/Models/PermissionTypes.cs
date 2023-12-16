using System.ComponentModel.DataAnnotations;

namespace n5.Infrastructure.Models;
public class PermissionTypes
{
    [Key]
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
}
