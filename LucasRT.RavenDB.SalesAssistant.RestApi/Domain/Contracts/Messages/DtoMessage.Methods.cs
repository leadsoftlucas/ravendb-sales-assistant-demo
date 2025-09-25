using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Messages;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Contracts.Messages
{
    public partial class DtoMessage
    {
        public static DtoMessage GetPersonal(AIEmailSuggestions emailSuggestion)
        {
            if (emailSuggestion is null || emailSuggestion.PersonalRecipients.IsNothing())
                return null;

            return new()
            {
                Recipient = emailSuggestion.PersonalRecipients,
                Subject = emailSuggestion.PersonalSubject,
                Body = emailSuggestion.PersonalBody
            };
        }

        public static DtoMessage GetProfessional(AIEmailSuggestions emailSuggestion)
        {
            if (emailSuggestion is null || emailSuggestion.ProfessionalRecipients.IsNothing())
                return null;

            return new()
            {
                Recipient = emailSuggestion.ProfessionalRecipients,
                Subject = emailSuggestion.ProfessionalSubject,
                Body = emailSuggestion.ProfessionalBody
            };
        }
    }
}
