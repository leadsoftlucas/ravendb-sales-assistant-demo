using LeadSoft.Common.GlobalDomain.DTOs;
using LeadSoft.Common.Library.Constants;
using LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Messages;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Contracts.Messages;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        /// <summary>
        /// Retrieves a list of messages based on the specified lead origin and pagination parameters, and returns them as an HTML string.
        /// </summary>
        /// <remarks>
        /// The method returns an HTML content type response. Ensure that the pagination parameters are valid to avoid exceptions.
        /// 
        /// I recommend you to call this method directly from a web browser or a tool that can render HTML content, as it returns an HTML string.
        /// </remarks>
        /// <param name="leadOrigin">The origin of the leads for which messages are to be retrieved. This parameter determines the source context
        /// of the messages.</param>
        /// <param name="dtoPage">The pagination parameters that specify the page number and size for the message retrieval. This helps in
        /// fetching a specific subset of messages.</param>
        /// <returns>An HTML string representing the list of messages formatted according to the specified lead origin and
        /// pagination parameters.</returns>
        [SwaggerOperation(summary: "Retrieves a list of messages based on the specified lead origin and pagination parameters, and returns them as an HTML string.", Description = "Handles HTTP GET requests to fetch messages.")]
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

        /// <summary>
        /// Creates a new message template using the specified data.
        /// </summary>
        /// <remarks>This method handles HTTP POST requests to create a new message template. It returns a
        /// 200 OK response with a <see cref="DTOBoolResponse"/> if the template is created successfully, or a 400 Bad
        /// Request if the input data is invalid.</remarks>
        /// <param name="dtoInsert">The data transfer object containing the details of the message template to be created. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="DTOBoolResponse"/>
        /// indicating the success of the operation.</returns>
        [SwaggerOperation(summary: "Creates a new message template using the specified data.", Description = "Handles HTTP POST requests to create a new message template.")]
        [HttpPost("Template", Name = nameof(PostMessageTemplateAsync))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOBoolResponse), StatusCodes.Status200OK)]
        [Produces(Constant.ApplicationProblemJson)]
        [Consumes(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOBoolResponse>> PostMessageTemplateAsync([FromBody] DtoMessageTemplateInsert dtoInsert)
            => Ok(await messages.CreateMessageTemplateAsync(dtoInsert));
    }
}
