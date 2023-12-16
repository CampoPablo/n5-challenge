using n5.Application.Dto;
using n5.WebApi.Model;

namespace n5.WebApi.Mapper;

/// <summary>
/// Este helper transforma un objeto de tipo <see cref="IList{PermissionDto}"/> en un objeto de tipo <see cref="GetPermissionsResult"/>
/// </summary>

public static class ConvertPermissionsDtoMapper
{
    public static void ToGetPermissionsResult(IList<PermissionDto> source, GetPermissionsResult destination)
    {
        foreach (var item in source)
        {
            var permission = new GetPermissionResult
            {
                EmployeeId = item.EmployeeId,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Department = item.Department,
                PermissionTypeId = item.PermissionTypeId,
                Code = item.CodePermission,
                Description = item.DescriptionPermission
            };
            destination.Permissions.Add(permission);
        }
    }
}
