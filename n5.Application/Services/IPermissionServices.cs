using n5.Application.Dto;

namespace n5.Application.Services
{
    public interface IPermissionServices
    {
        public bool RequestPermission(RequestPermissionDto permission);

        public void ModifyPermission(Guid employeeId, Guid permissionTypeId, Guid newPermissionTypeId);

        public PermissionDto GetPermissionById(Guid id);

        public IList<PermissionDto> GetPermissionsByEmployeeId(Guid employeeId);
    }
}