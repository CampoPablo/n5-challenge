namespace n5.WebApi.Model
{
    public class GetPermissionResult
    {
        public Guid EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; } 
        public string? Department { get; set; }
        public Guid PermissionTypeId { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; } 
    }

    public class GetPermissionsResult
    {
        public IList<GetPermissionResult> Permissions { get; set; } = new List<GetPermissionResult>();
        public ResultData ResultData { get; set; } = new ResultData();
    }
}
