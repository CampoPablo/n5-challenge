using n5.Application.Dto;
using n5.Infrastructure.Models;
using n5.Infrastructure.Repository;
using n5.Infrastructure.ElasticSearch;
using n5.Application.Services.Models;
using n5.Application.Services.Exceptions;
using n5.Infrastructure.UnitOfWork;
using n5.Infrastructure.Kafka;

namespace n5.Application.Services
{
    /// <summary>
    /// This class is used to manage the permissions of the employees
    /// </summary>
    public class PermissionServices : IPermissionServices
    {
        private readonly IRepository<PermissionTypes> _permissionTypesRepository;
        private readonly IRepository<Employees> _employeesRepository;
        private readonly IRepository<Permissions> _permissionsRepository;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKafkaProducer _kafkaProducer;

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="permissionTypesRepository"></param>
        /// <param name="employeesRepository"></param>
        /// <param name="permissionsRepository"></param>
        /// <param name="elasticSearchService"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="kafkaProducer"></param>
        public PermissionServices(IRepository<PermissionTypes> permissionTypesRepository, IRepository<Employees> employeesRepository,
        IRepository<Permissions> permissionsRepository, IElasticSearchService elasticSearchService, IUnitOfWork unitOfWork, IKafkaProducer kafkaProducer)
        {
            _permissionTypesRepository = permissionTypesRepository;
            _employeesRepository = employeesRepository;
            _permissionsRepository = permissionsRepository;
            _elasticSearchService = elasticSearchService;
            _unitOfWork = unitOfWork;
            _kafkaProducer = kafkaProducer;
        }

        /// <summary>
        /// This method is used to get a permission by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public PermissionDto GetPermissionById(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to get all permissions by employee id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public IList<PermissionDto> GetPermissionsByEmployeeId(Guid employeeId)
        {
            var dataEmployees = _employeesRepository.Find(x => x.Id == employeeId).FirstOrDefault();
            var dataPermissions = _permissionsRepository.Find(x => x.EmployeeId == employeeId);
            var dataPermissionTypes = _permissionTypesRepository.GetAll();

            IList<PermissionDto> permissions = new List<PermissionDto>();
            if (dataEmployees != null && dataPermissions.Count() > 0 && dataPermissionTypes.Count() > 0)
            {
                foreach (var item in dataPermissions)
                {
                    var permission = new PermissionDto();
                    permission.Id = item.Id;
                    permission.EmployeeId = item.EmployeeId;
                    permission.PermissionTypeId = item.PermissionTypeId;
                    permission.FirstName = dataEmployees.FirstName;
                    permission.LastName = dataEmployees.LastName;
                    permission.Department = dataEmployees.Department;
                    permission.IdentificationDocument = dataEmployees.IdentificationDocument;
                    permission.CodePermission = dataPermissionTypes.Where(x => x.Id == item.PermissionTypeId).First().Code;
                    permission.DescriptionPermission = dataPermissionTypes.Where(x => x.Id == item.PermissionTypeId).First().Description;
                    permissions.Add(permission);
                }
            }
            ElasticDocument document = new ElasticDocument("GetPermissionsByEmployeeId", $"EmployeeId: {employeeId}");
            _elasticSearchService.IndexDocumentAsync("n5-indice", "GetPermissionsByEmployeeId", document);
            OperationMessage operationMessage = new OperationMessage
            {
                Id = Guid.NewGuid(),
                NameOperation = "Get"
            };

            _kafkaProducer.Produce("operation-Get", operationMessage);
            return permissions;
        }

        /// <summary>
        ///  This method is used to mofify a new permission to an employee      
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="permissionTypeId"></param>
        /// <param name="newPermissionTypeId"></param>
        public void ModifyPermission(Guid employeeId, Guid permissionTypeId, Guid newPermissionTypeId)
        {
            var permissionData = _permissionsRepository.Find(x => x.EmployeeId == employeeId && x.PermissionTypeId == permissionTypeId).FirstOrDefault();
            ValidatePermissionException.IfNotExistsPemissionsThrowException(permissionData);

            permissionData.PermissionTypeId = newPermissionTypeId;
            _permissionsRepository.Update(permissionData);
            _unitOfWork.Commit();

            ElasticDocument document = new ElasticDocument("Modify Permission", $"EmployeeId: {permissionData.EmployeeId}, PermissionTypeId: {newPermissionTypeId}");
            _elasticSearchService.IndexDocumentAsync("n5-indice", "ModifyPermission", document);

            OperationMessage operationMessage = new OperationMessage
            {
                Id = Guid.NewGuid(),
                NameOperation = "Modify"
            };

            _kafkaProducer.Produce("operation-Modify", operationMessage);
        }

        /// <summary>
        ///    This method is used to request for a permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public bool RequestPermission(RequestPermissionDto permission)
        {
            var data = _permissionsRepository.Find(x => x.EmployeeId == permission.EmployeeId && x.PermissionTypeId == permission.PermissionTypeId);
            ElasticDocument document = new ElasticDocument("Permission Request", $"EmployeeId: {permission.EmployeeId}, PermissionTypeId: {permission.PermissionTypeId}");
            _elasticSearchService.IndexDocumentAsync("n5-indice", "RequestPermission", document);
            OperationMessage operationMessage = new OperationMessage
            {
                Id = Guid.NewGuid(),
                NameOperation = "Request"
            };
            _kafkaProducer.Produce("operation-Request", operationMessage);
            return data.Count() > 0;
        }
    }
}