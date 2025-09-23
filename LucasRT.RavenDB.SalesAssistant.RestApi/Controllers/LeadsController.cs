using LeadSoft.Common.GlobalDomain.DTOs;
using LeadSoft.Common.Library;
using LeadSoft.Common.Library.Constants;
using LeadSoft.Common.Library.Extensions.Attributes;
using LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Leads;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeadsController(ILeadService leads, ILogger<LeadsController> logger) : ControllerBase
    {
        [HttpPost("", Name = nameof(PostLeadsAsync))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status411LengthRequired)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(DTOBoolResponse), StatusCodes.Status200OK)]
        [Produces(Constant.ApplicationProblemJson)]
        [Consumes(Constant.MultipartFormData)]
        public async Task<ActionResult<DTOBoolResponse>> PostLeadsAsync([AllowedFileExtensions([".csv"])][Required(ErrorMessage = Constant.RequiredField)] IFormFile csvFile)
        {
            if (HttpContext.Request.Form.Files.Count != 1)
                return BadRequest(ApplicationStatusMessage.OneFileAtOnce);

            if (csvFile.Length <= 0)
                return StatusCode(411, ApplicationStatusMessage.ReadableBytes);

            return Ok(await leads.ImportAsync(csvFile));
        }
    }
}
