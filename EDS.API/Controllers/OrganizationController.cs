using EDS.API.Controllers.Shared;
using EDS.API.ViewModels;
using EDS.Common.Utilities;
using EDS.Services.Services;
using HMIS.IM.API.ViewModels.Shared;
using HMIS.IM.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using System.Net;

namespace EDS.API.Controllers
{
    public class OrganizationController : BaseApiController
    {
        private readonly OrganizationService _service;
        //private readonly ILogger<object> _logger;
        private readonly TenantConfig _currentTenant;


        public OrganizationController(IOptions<TenantConfig> options)
        {
            _currentTenant = options.Value;
            _service = new OrganizationService(_currentTenant);
            //_logger = loggerFactory.CreateLogger<object>();
        }
        [HttpGet("GetAllOrganizations")]
        public async Task<ActionResult<ApiGridResponse<OrganizationVM>>> GetAllOrganizations() 
        { 
            var reponse = new ApiGridResponse<OrganizationVM>();
            try
            {
                var orgList = _service.GetAllOrganizations();
                if (orgList != null && orgList.Any())
                {
                    return Ok(reponse.GetSuccessResponseObject(new OrganizationVM().FromServiceModelList(orgList).ToList(), Constant.GET_API_SUCCESS_MSG));
                }
                else
                {
                    return Ok(reponse.GetNullResponseObject());
                }

            }
            catch (Exception exp)
            {
                return BadRequest(reponse.GetErrorResponseObject((int)HttpStatusCode.InternalServerError, ErrorCodes.SYSTEM_ERROR, exp.Message));
                throw;
            }
        }
    }
}
