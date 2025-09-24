namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Messages
{
    public partial class AIEmailSuggestions
    {
        public string ProfessionalRecipients { get; init; } = string.Empty;
        public string ProfessionalSubject { get; init; } = string.Empty;
        public string ProfessionalBody { get; init; } = string.Empty;
        public string PersonalRecipients { get; init; } = string.Empty;
        public string PersonalSubject { get; init; } = string.Empty;
        public string PersonalBody { get; init; } = string.Empty;
    }
}
