
namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Messages
{
    /// <summary>
    /// Provides AI-generated email suggestions tailored for both professional and personal contexts.
    /// </summary>
    /// <remarks>This class offers properties to hold email recipients, subject, and body content for both
    /// professional and personal emails. The suggestions are designed to assist in formatting and translating email
    /// content based on the context and presence of email addresses.</remarks>
    public partial class AIEmailSuggestions
    {
        /// <summary>
        /// Gets the list of professional recipients.
        /// </summary>
        public string ProfessionalRecipients { get; init; } = string.Empty;

        /// <summary>
        /// Gets the professional subject associated with the entity.
        /// </summary>
        public string ProfessionalSubject { get; init; } = string.Empty;

        /// <summary>
        /// Gets the name of the professional body associated with the entity.
        /// </summary>
        public string ProfessionalBody { get; init; } = string.Empty;

        /// <summary>
        /// Gets the list of personal recipients.
        /// </summary>
        public string PersonalRecipients { get; init; } = string.Empty;

        /// <summary>
        /// Gets the personal subject associated with the current instance.
        /// </summary>
        public string PersonalSubject { get; init; } = string.Empty;

        /// <summary>
        /// Gets the personal body text associated with the entity.
        /// </summary>
        public string PersonalBody { get; init; } = string.Empty;

        /// <summary>
        /// Generates AI email suggestions for both professional and personal contexts.
        /// </summary>
        /// <returns>An <see cref="AIEmailSuggestions"/> object containing instructions for generating email recipients,  body,
        /// and subject for both professional and personal emails. The instructions specify how to format  and translate
        /// the email content based on the presence of email addresses and the context of the message.</returns>
        public static AIEmailSuggestions GetAIFieldsInstructions()
            => new()
            {
                ProfessionalRecipients = "All professional e-mails semicolon separated. Blank if none.",
                ProfessionalBody = "If there are Professional E-mails, fill lead's information to complete the message with Name and maybe lead's company somewhere strategic. Translate the e-mail body template text to the lead's language and apply the voice target on the text without modifying the core of the message and keep it corporative. Otherwise leave it blank.",
                ProfessionalSubject = "If there are Professional E-mails, based on Professional Body message, create a nice and atractive about new GenAI RavenDB features announcement short e-mail subject on professional company context, otherwise leave it blank.",
                PersonalRecipients = "All personal e-mails semicolon separated. Blank if none.",
                PersonalBody = "If there are Personal E-mails, fill lead's information to complete the message with Name. Translate the e-mail body template text to the lead's language and apply the voice target on the text without modifying the core of the message and humanize it. Otherwise leave it blank.",
                PersonalSubject = "If there are Personal E-mails, based on Personal Body message, create a nice and atractive about new GenAI RavenDB features announcement short e-mail subject on personal friendly approach, otherwise leave it blank."
            };
    }
}
