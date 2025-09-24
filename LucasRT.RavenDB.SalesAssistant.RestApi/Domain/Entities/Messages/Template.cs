using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Messages
{
    public partial class Template
    {
        public string Id { get; set; } = null;
        public LeadOrigin Destination { get; set; } = LeadOrigin.Other;
        public string Text { get; set; } = string.Empty;
    }
}
