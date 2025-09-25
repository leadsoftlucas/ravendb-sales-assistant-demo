
namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads
{
    /// <summary>
    /// Represents a set of instructions for generating AI field values based on specific criteria.
    /// </summary>
    /// <remarks>The <see cref="AiKnowledge"/> class provides properties to define guidelines for populating
    /// AI fields, including names, email addresses, company identification, and language specifications.</remarks>
    public partial class AiKnowledge
    {
        /// <summary>
        /// Gets the name of the company associated with the entity.
        /// </summary>
        public string Company { get; init; } = string.Empty;

        /// <summary>
        /// Gets the user-friendly name associated with the object.
        /// </summary>
        public string FriendlyName { get; init; } = string.Empty;

        /// <summary>
        /// Gets the personal email addresses associated with the user.
        /// </summary>
        public string PersonalEmails { get; init; } = string.Empty;

        /// <summary>
        /// Gets the professional email address associated with the user.
        /// </summary>
        public string ProfessionalEmail { get; init; } = string.Empty;

        /// <summary>
        /// Gets the spoken language associated with the entity.
        /// </summary>
        public string SpokenLanguage { get; init; } = string.Empty;

        /// <summary>
        /// Gets the target voice identifier for the current operation.
        /// </summary>
        public string VoiceTarget { get; init; } = string.Empty;

        /// <summary>
        /// Provides instructions for generating AI field values based on specific criteria.
        /// </summary>
        /// <remarks>This method returns an <see cref="AiKnowledge"/> object with predefined instructions
        /// for each field: <list type="bullet"> <item> <description> <strong>FriendlyName:</strong> Use only the first
        /// and main composed names, avoiding surnames. </description> </item> <item> <description>
        /// <strong>VoiceTarget:</strong> Generate a writing target instruction based on age and gender, if available.
        /// Use a neutral professional approach if this information is not provided. </description> </item> <item>
        /// <description> <strong>PersonalEmails:</strong> Include the best personal generic emails, separated by
        /// semicolons. Avoid professional emails or those not belonging to the person. </description> </item> <item>
        /// <description> <strong>ProfessionalEmail:</strong> Include a professional or student email associated with a
        /// company domain. If the domain suggests a company or university, use it; otherwise, leave it blank.
        /// </description> </item> <item> <description> <strong>Company:</strong> Derive the company name from the
        /// professional email domain. Use "Student" for university domains, or leave blank if indeterminate.
        /// </description> </item> <item> <description> <strong>SpokenLanguage:</strong> Specify the spoken language and
        /// cultural context. </description> </item> </list></remarks>
        /// <returns>An <see cref="AiKnowledge"/> object containing detailed instructions for populating AI fields, including
        /// guidelines for names, email addresses, company identification, and language specifications.</returns>
        public static AiKnowledge GetAIFieldsInstructions()
            => new()
            {
                FriendlyName = "Use his first and main composed names here only. Avoid surnames.",
                VoiceTarget = "Based on Age and Gender, if there are any, choose the best fit person generation. Create a writing target instruction based on gender and generation. Use neutral professional approach if you don't have this info.",
                PersonalEmails = "The best personal generic e-mails should be here semicolon separated. Avoid his professional e-mail or e-mails that seems not to belong to this person.",
                ProfessionalEmail = "If it has a professional or student e-mail address associated with the company by domain, place here. The company name could be in the address for some generic e-mail providers.",
                Company = "Use the Company Name field of some professional e-mail domain to find the company. If you find an university domain, use Student. If you can't say if it is a company or student, leave it blank.",
                SpokenLanguage = "Place here the spoken language and country culture specification."
            };
    }
}
