using LeadSoft.Common.GlobalDomain.DTOs;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Contracts.Messages;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Messages;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Messages
{
    public partial class MessageService(IDocumentStore ravenDB) : IMessageService
    {
        public async Task<IList<DtoMessageResponse>> ListMessagesAsync(LeadOrigin leadOrigin, DTOPagedRequest dtoPage)
        {
            IAsyncDocumentSession session = ravenDB.OpenAsyncSession();

            int currentPage = Math.Abs(dtoPage.CurrentPage);
            if (currentPage == 0)
                currentPage = 1;

            IList<Message> messages = await session.Query<Message>()
                                                   .Include(m => m.LeadId)
                                                   .Where(m => m.Origin == leadOrigin && m.AIEmailSuggestions != null)
                                                   .OrderBy(m => m.AiKnowledge.FriendlyName)
                                                   .Skip(dtoPage.CurrentPage * dtoPage.PageSize)
                                                   .Take(dtoPage.PageSize)
                                                   .ToListAsync();

            IList<Lead> leads = await session.Query<Lead>()
                                             .Where(l => l.Id.In(messages.Select(m => m.LeadId).Distinct()))
                                             .ToListAsync();

            return DtoMessageResponse.ListOf(messages, leads);
        }

        public async Task<DTOBoolResponse> CreateMessageTemplateAsync(DtoMessageTemplateInsert dtoInsert)
        {
            IAsyncDocumentSession session = ravenDB.OpenAsyncSession();

            await session.StoreAsync(new Template()
            {
                Destination = dtoInsert.Destination,
                Text = dtoInsert.Text.Trim()
            });

            await session.SaveChangesAsync();

            return new(true);
        }
    }
}
