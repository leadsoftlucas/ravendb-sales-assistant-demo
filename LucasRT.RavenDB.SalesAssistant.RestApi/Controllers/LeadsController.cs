using LeadSoft.Common.GlobalDomain.DTOs;
using LeadSoft.Common.Library;
using LeadSoft.Common.Library.Constants;
using LeadSoft.Common.Library.Extensions.Attributes;
using LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Leads;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Controllers
{
    /// <summary>
    /// Provides endpoints for managing and importing lead data.
    /// </summary>
    /// <remarks>The <see cref="LeadsController"/> class handles HTTP requests related to lead data
    /// operations, including importing leads from CSV files. It supports multipart form-data uploads and ensures that
    /// the uploaded file meets specific criteria before processing.</remarks>
    /// <param name="leads"></param>
    /// <param name="logger"></param>
    [ApiController]
    [Route("[controller]")]
    //[SwaggerTag("Leads", "Operations related to lead data management and import.")]
    public class LeadsController(ILeadService leads, ILogger<LeadsController> logger) : ControllerBase
    {
        /// <summary>
        /// Processes and imports lead data from a CSV file asynchronously.
        /// </summary>
        /// <remarks>This method supports multipart form-data content type and expects exactly one file to
        /// be uploaded. It returns a 400 Bad Request if more than one file is provided, a 411 Length Required if the
        /// file is empty, and a 422 Unprocessable Entity if the file cannot be processed.</remarks>
        /// <param name="csvFile">The CSV file containing lead data to be imported. Must be a non-empty file with a .csv extension.</param>
        /// <param name="leadOrigin">The origin of the leads being imported. Defaults to <see cref="LeadOrigin.Other"/> if not specified.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="DTOBoolResponse"/> indicating the success of the
        /// import operation.</returns>
        [HttpPost("", Name = nameof(PostLeadsAsync))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status411LengthRequired)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(DTOBoolResponse), StatusCodes.Status200OK)]
        [Produces(Constant.ApplicationProblemJson)]
        [Consumes(Constant.MultipartFormData)]
        public async Task<ActionResult<DTOBoolResponse>> PostLeadsAsync([AllowedFileExtensions([".csv"])][Required(ErrorMessage = Constant.RequiredField)] IFormFile csvFile, [FromQuery] CultureName cultureName = CultureName.pt_BR, [FromQuery] LeadOrigin leadOrigin = LeadOrigin.Other)
        {
            if (HttpContext.Request.Form.Files.Count != 1)
                return BadRequest(ApplicationStatusMessage.OneFileAtOnce);

            if (csvFile.Length <= 0)
                return StatusCode(411, ApplicationStatusMessage.ReadableBytes);

            return Ok(await leads.ImportAsync(csvFile, cultureName, leadOrigin));
        }
    }
}
