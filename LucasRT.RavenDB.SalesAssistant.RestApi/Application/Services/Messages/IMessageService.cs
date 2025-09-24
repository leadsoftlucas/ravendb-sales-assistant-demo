using LeadSoft.Common.GlobalDomain.DTOs;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Contracts.Messages;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Messages
{
    public interface IMessageService
    {
        Task<IList<DtoMessageResponse>> ListMessagesAsync(LeadOrigin leadOrigin, DTOPagedRequest dtoPage);
        Task<DTOBoolResponse> CreateMessageTemplateAsync(DtoMessageTemplateInsert dtoInsert);
    }
}
