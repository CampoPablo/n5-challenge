using System.Linq.Expressions;
using Moq;
using n5.Application.Dto;
using n5.Application.Services;
using n5.Infrastructure.ElasticSearch;
using n5.Infrastructure.Kafka;
using n5.Infrastructure.Models;
using n5.Infrastructure.Repository;
using n5.Infrastructure.UnitOfWork;
using n5.WebApi.Model;
using Xunit;

namespace n5.Tests
{
    public class PermissionServicesTests
    {
        private readonly PermissionServices _permissionServices;
        private readonly Mock<IRepository<PermissionTypes>> _permissionTypesRepositoryMock;
        private readonly Mock<IRepository<Employees>> _employeesRepositoryMock;
        private readonly Mock<IRepository<Permissions>> _permissionsRepositoryMock;
        private readonly Mock<IElasticSearchService> _elasticSearchServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IKafkaProducer> _kafkaProducerMock;


        public PermissionServicesTests()
        {
            _permissionTypesRepositoryMock = new Mock<IRepository<PermissionTypes>>();
            _employeesRepositoryMock = new Mock<IRepository<Employees>>();
            _permissionsRepositoryMock = new Mock<IRepository<Permissions>>();
            _elasticSearchServiceMock = new Mock<IElasticSearchService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _permissionServices = new PermissionServices(
                _permissionTypesRepositoryMock.Object,
                _employeesRepositoryMock.Object,
                _permissionsRepositoryMock.Object,
                _elasticSearchServiceMock.Object,
                _unitOfWorkMock.Object,
                _kafkaProducerMock.Object
            );
        }

        [Fact]
        public void GetPermissionsByEmployeeId_ShouldReturnPermissions()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var permissionTypeId = Guid.NewGuid();
            var permission = new Permissions
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                PermissionTypeId = permissionTypeId
            };
            var employee = new Employees
            {
                Id = employeeId,
                FirstName = "John",
                LastName = "Doe",
                Department = "IT",
                IdentificationDocument = "1234567890"
            };
            var permissionType = new PermissionTypes
            {
                Id = permissionTypeId,
                Code = "PERM001",
                Description = "Permission 1"
            };

            _employeesRepositoryMock.Setup(x => x.Find(It.IsAny<Expression<Func<Employees, bool>>>())).Returns(new List<Employees> { employee });
            _permissionsRepositoryMock.Setup(x => x.Find(It.IsAny<Expression<Func<Permissions, bool>>>())).Returns(new List<Permissions> { permission });
            _permissionTypesRepositoryMock.Setup(x => x.GetAll()).Returns(new List<PermissionTypes> { permissionType });

            // Act
            var result = _permissionServices.GetPermissionsByEmployeeId(employeeId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(permission.Id, result[0].Id);
            Assert.Equal(employeeId, result[0].EmployeeId);
            Assert.Equal(permissionTypeId, result[0].PermissionTypeId);
            Assert.Equal(employee.FirstName, result[0].FirstName);
            Assert.Equal(employee.LastName, result[0].LastName);
            Assert.Equal(employee.Department, result[0].Department);
            Assert.Equal(employee.IdentificationDocument, result[0].IdentificationDocument);
            Assert.Equal(permissionType.Code, result[0].CodePermission);
            Assert.Equal(permissionType.Description, result[0].DescriptionPermission);
        }

        [Fact]
        public void ModifyPermission_ShouldUpdatePermission()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var permissionTypeId = Guid.NewGuid();
            var newPermissionTypeId = Guid.NewGuid();
            var permission = new Permissions
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                PermissionTypeId = permissionTypeId
            };

            _permissionsRepositoryMock.Setup(x => x.Find(It.IsAny<Expression<Func<Permissions, bool>>>())).Returns(new List<Permissions> { permission });

            // Act
            _permissionServices.ModifyPermission(employeeId, permissionTypeId, newPermissionTypeId);

            // Assert
            _permissionsRepositoryMock.Verify(x => x.Update(It.Is<Permissions>(p => p.EmployeeId == employeeId && p.PermissionTypeId == newPermissionTypeId)), Times.Once);
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public void RequestPermission_ShouldReturnTrueIfPermissionExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var permissionTypeId = Guid.NewGuid();
            var permission = new Permissions
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                PermissionTypeId = permissionTypeId
            };

            _permissionsRepositoryMock.Setup(x => x.Find(It.IsAny<Expression<Func<Permissions, bool>>>())).Returns(new List<Permissions> { permission });

            var requestPermissionDto = new RequestPermissionDto
            {
                EmployeeId = employeeId,
                PermissionTypeId = permissionTypeId
            };

            // Act
            var result = _permissionServices.RequestPermission(requestPermissionDto);

            // Assert
            Assert.True(result);
        }
    }
}