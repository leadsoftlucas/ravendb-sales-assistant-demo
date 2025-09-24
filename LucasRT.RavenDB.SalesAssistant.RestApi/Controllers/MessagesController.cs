using LeadSoft.Common.GlobalDomain.DTOs;
using LeadSoft.Common.Library.Constants;
using LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Messages;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Contracts.Messages;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using Microsoft.AspNetCore.Mvc;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Controllers
{
    /// <summary>
    /// Provides endpoints for managing messages.
    /// </summary>
    /// <remarks>This controller handles HTTP requests related to message operations, such as creating new
    /// messages.</remarks>
    /// <param name="messages"></param>
    /// <param name="logger"></param>
    [ApiController]
    [Route("[controller]")]
    //[SwaggerTag("Messages", "Provides endpoints for managing messages.")]
    public class MessagesController(IMessageService messages, ILogger<MessagesController> logger) : ControllerBase
    {
        [HttpGet("", Name = nameof(GetMessagesAsync))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Produces("text/html")]
        public async Task<ActionResult<string>> GetMessagesAsync([FromQuery] LeadOrigin leadOrigin, [FromQuery] DTOPagedRequest dtoPage)
        {
            IList<DtoMessageResponse> dto = await messages.ListMessagesAsync(leadOrigin, dtoPage);

            string html = DtoMessageResponse.ToHtml(dto, new DtoMessageResponse.HtmlRenderOptions
            {
                Title = $"Mensagens - {leadOrigin}",
                IncludeStyles = true
            });

            return Content(html, "text/html; charset=utf-8");
        }

        [HttpPost("Template", Name = nameof(PostMessageTemplateAsync))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOBoolResponse), StatusCodes.Status200OK)]
        [Produces(Constant.ApplicationProblemJson)]
        [Consumes(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOBoolResponse>> PostMessageTemplateAsync([FromBody] DtoMessageTemplateInsert dtoInsert)
        {
            return Ok(await messages.CreateMessageTemplateAsync(dtoInsert));
        }
    }
}
