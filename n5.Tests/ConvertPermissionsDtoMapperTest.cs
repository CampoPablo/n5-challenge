using n5.Application.Dto;
using n5.WebApi.Mapper;
using n5.WebApi.Model;

namespace n5.WebApi.Tests.Mapper
{
    namespace n5.WebApi.Tests.Mapper
    {
        public class ConvertPermissionsDtoMapperTests
        {
            [Fact]
            public void ToGetPermissionsResult_ShouldMapPermissionsDtoToGetPermissionsResult()
            {
                // Arrange
                var source = new List<PermissionDto>
                {
                    new PermissionDto
                    {
                        EmployeeId = Guid.Parse("{12345678-1234-1234-1234-123456789aec}"),
                        FirstName = "John",
                        LastName = "Doe",
                        Department = "IT",
                        PermissionTypeId = Guid.Parse("{12345678-1234-1234-1234-123456789aaa}"),
                        CodePermission = "PERM001",
                        DescriptionPermission = "Permission 1"
                    },
                    new PermissionDto
                    {
                        EmployeeId = Guid.Parse("{12345678-1234-1234-1234-123456789aed}"),
                        FirstName = "Jane",
                        LastName = "Smith",
                        Department = "HR",
                        PermissionTypeId = Guid.Parse("{12345678-1234-1234-1234-123456789abb}"),
                        CodePermission = "PERM002",
                        DescriptionPermission = "Permission 2"
                    }
                };

                var destination = new GetPermissionsResult();

                // Act
                ConvertPermissionsDtoMapper.ToGetPermissionsResult(source, destination);

                // Assert
                
                Assert.Equal(2, destination.Permissions.Count);
                Assert.Equal(Guid.Parse("{12345678-1234-1234-1234-123456789aec}"), destination.Permissions[0].EmployeeId);
                Assert.Equal("John", destination.Permissions[0].FirstName);
                Assert.Equal("Doe", destination.Permissions[0].LastName);
                Assert.Equal("IT", destination.Permissions[0].Department);
                Assert.Equal(Guid.Parse("{12345678-1234-1234-1234-123456789aaa}"), destination.Permissions[0].PermissionTypeId);
                Assert.Equal("PERM001", destination.Permissions[0].Code);
                Assert.Equal("Permission 1", destination.Permissions[0].Description);
                Assert.Equal(Guid.Parse("{12345678-1234-1234-1234-123456789aed}"), destination.Permissions[1].EmployeeId);
                Assert.Equal("Jane", destination.Permissions[1].FirstName);
                Assert.Equal("Smith", destination.Permissions[1].LastName);
                Assert.Equal("HR", destination.Permissions[1].Department);
                Assert.Equal(Guid.Parse("{12345678-1234-1234-1234-123456789abb}"), destination.Permissions[1].PermissionTypeId);
                Assert.Equal("PERM002", destination.Permissions[1].Code);
                Assert.Equal("Permission 2", destination.Permissions[1].Description);
            }
        }
    }
    
}