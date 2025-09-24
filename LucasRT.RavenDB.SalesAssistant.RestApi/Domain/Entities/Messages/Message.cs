using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Messages
{
    public partial class Message
    {
        public string Id { get; set; } = null;
        public string LeadId { get; set; } = string.Empty;
        public LeadOrigin Origin { get; set; } = LeadOrigin.Other;
        public AiKnowledge AiKnowledge { get; set; }
        public Template Template { get; set; }
        public AIEmailSuggestions AIEmailSuggestions { get; set; }
    }
}
