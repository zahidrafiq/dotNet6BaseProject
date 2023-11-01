using EDS.API.Controllers.Shared;
using EDS.Common.Utilities;
using EDS.Services.Services;
using HMIS.IM.API.ViewModels.Shared;
using HMIS.IM.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace EDS.API.Controllers
{
    public class ClientController : BaseApiController
    {
        //private readonly ClientService _service;
        //private readonly ILogger<object> _logger;
        private readonly TenantConfig _currentTenant;


        public ClientController(IOptions<TenantConfig> options)
        {
            _currentTenant = options.Value;
            //_logger = loggerFactory.CreateLogger<object>();
        }

        [HttpGet("GetAllClients")]
        public async Task<ActionResult<ApiGridResponse<FilterModel>>> GetAllClients()
        {
            ClientService _service = new ClientService(_currentTenant);
            var reponse = new ApiGridResponse<FilterModel>();
            try
            {
                var clientList = _service.GetAllClients();
                if (clientList != null && clientList.Any())
                {
                    return Ok(reponse.GetSuccessResponseObject(clientList, Constant.GET_API_SUCCESS_MSG));
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
