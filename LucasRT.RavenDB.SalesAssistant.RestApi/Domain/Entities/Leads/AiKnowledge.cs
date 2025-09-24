namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads
{
    public partial class AiKnowledge
    {
        public string Company { get; init; } = string.Empty;
        public string FriendlyName { get; init; } = string.Empty;
        public string PersonalEmails { get; init; } = string.Empty;
        public string ProfessionalEmail { get; init; } = string.Empty;
        public string SpokenLanguage { get; init; } = string.Empty;
        public string VoiceTarget { get; init; } = string.Empty;
    }
}
