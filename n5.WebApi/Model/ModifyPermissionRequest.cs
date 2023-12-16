namespace n5.WebApi.Model;

public class ModifyPermissionRequest
{
    public Guid EmployeeId { get; set; }
    public Guid OldPermissionType { get; set; }
    public Guid NewPermissionType { get; set; }
}
