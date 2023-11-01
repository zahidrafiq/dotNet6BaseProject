using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EDS.API.Controllers.Shared
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    //[Route("/api/v{v:apiVersion}/[controller]")]
    //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    //[ServiceFilter(typeof(ClaimsFilter))]
    public class BaseApiController : ControllerBase
    {
    }
}
