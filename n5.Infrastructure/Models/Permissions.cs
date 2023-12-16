using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace n5.Infrastructure.Models;

public class Permissions
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("IdentityEmploee")]
    public Guid EmployeeId { get; set; }

    [ForeignKey("IdentityPermissionType")]
    public Guid PermissionTypeId { get; set; }
}
