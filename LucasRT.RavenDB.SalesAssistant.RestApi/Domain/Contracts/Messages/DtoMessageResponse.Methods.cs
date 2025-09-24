using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Messages;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Contracts.Messages
{
    public partial class DtoMessageResponse
    {
        public static implicit operator DtoMessageResponse(Message message)
        {
            if (message == null)
                return null;

            return new DtoMessageResponse()
            {
                Id = message.Id,
                LeadId = message.LeadId,
                LeadOrigin = message.Origin,
                Personal = DtoMessage.GetPersonal(message.AIEmailSuggestions),
                Professional = DtoMessage.GetProfessional(message.AIEmailSuggestions)
            };
        }

        public static IList<DtoMessageResponse> ListOf(IList<Message> messages, IList<Lead> leads)
        {
            IList<DtoMessageResponse> responses = [.. messages.Select(m => (DtoMessageResponse)m)];

            foreach (DtoMessageResponse response in responses)
            {
                Lead lead = leads.SingleOrDefault(l => l.Id == response.LeadId);
                if (lead != null)
                {
                    response.LeadName = lead.Name;
                    response.CultureName = lead.CultureName;
                }
            }

            return responses;
        }
    }
}
