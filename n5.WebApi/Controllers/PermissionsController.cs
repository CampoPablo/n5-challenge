using Microsoft.AspNetCore.Mvc;
using n5.Application.Dto;
using n5.Application.Services;
using n5.WebApi.Mapper;
using n5.WebApi.Model;

namespace n5.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionServices _permissionServices;
        private readonly Serilog.ILogger _logger;

        public PermissionsController(Serilog.ILogger logger, IPermissionServices permissionServices)
        {
            _logger = logger;
            _permissionServices = permissionServices;
        }

        [HttpGet("RequestPermission")]
        public ActionResult<RequestPermissionResult> RequestPermission(Guid employeeId, Guid permissionTypeId)
        {
            _logger.Information("START: Requesting permission");
            var requestPermissionData = new RequestPermissionResult();
            try
            {
                var result = _permissionServices.RequestPermission(new RequestPermissionDto { EmployeeId = employeeId, PermissionTypeId = permissionTypeId });
                requestPermissionData.Access = result;
                requestPermissionData.ResultData.Success = true;
                _logger.Information("FINISH: Requesting permission");
                return requestPermissionData;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ERROR requesting permission");
                requestPermissionData.ResultData.Success = false;
                requestPermissionData.ResultData.Message = ex.Message;
                return requestPermissionData;
            }
        }

        [HttpPost("ModifyPermissions")]
        public ActionResult<ModifyPermissionResult> ModifyPermissions(ModifyPermissionRequest modifyPermissionRequest)
        {
            _logger.Information("START: Modifying permissions");
            ModifyPermissionResult modifyPermissionResult = new ModifyPermissionResult();
            try
            {
                // _permissionServices.ModifyPermission(modifyPermissionRequest);
                modifyPermissionResult.ResultData.Success = true;
                _logger.Information("FINISH: Modifying permissions");
                return modifyPermissionResult;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ERROR modifying permissions");
                modifyPermissionResult.ResultData.Success = false;
                modifyPermissionResult.ResultData.Message = ex.Message;
                return modifyPermissionResult;
            }
        }

        [HttpGet("GetPermissions")]
        public ActionResult<GetPermissionsResult> GetPermissions(Guid employeeId)
        {
            _logger.Information("START: Getting permissions");
            GetPermissionsResult getPermissionsResult = new GetPermissionsResult();
            try
            {
                IList<PermissionDto> permissions = _permissionServices.GetPermissionsByEmployeeId(employeeId);
                ConvertPermissionsDtoMapper.ToGetPermissionsResult(permissions, getPermissionsResult);
                getPermissionsResult.ResultData.Success = true;
                _logger.Information("FINISH: Getting permissions");
                return getPermissionsResult;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ERROR getting permissions");
                getPermissionsResult.ResultData.Success = false;
                getPermissionsResult.ResultData.Message = ex.Message;
                return getPermissionsResult;
            }
        }
    }
}
