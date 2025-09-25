using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Messages
{
    /// <summary>
    /// Represents an Artificial Document message with associated metadata and content.
    /// </summary>
    /// <remarks>
    /// This class is generated automatically by Leads Index, what makes it an artificial collection, linked directly to a Lead.
    /// The <see cref="Message"/> class encapsulates information about a message, including its unique identifier, the lead it is associated with, its origin, and any AI-generated content or suggestions.
    /// 
    /// Basic, it's a readonly collection that holds the AI generated content for a Lead, based on its Template and AiKnowledge properties. No need to create or update it manually.
    /// </remarks>
    public partial class Message
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; } = null;

        /// <summary>
        /// Gets or sets the unique identifier for a lead.
        /// </summary>
        public string LeadId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the origin of the lead.
        /// </summary>
        public LeadOrigin Origin { get; set; } = LeadOrigin.Other;

        /// <summary>
        /// Gets or sets the AI knowledge configuration for the system.
        /// </summary>
        public AiKnowledge AiKnowledge { get; set; }

        /// <summary>
        /// Gets or sets the template used for generating documents.
        /// </summary>
        public Template Template { get; set; }

        /// <summary>
        /// Gets or sets the AI-generated email suggestions.
        /// </summary>
        public AIEmailSuggestions AIEmailSuggestions { get; set; }
    }
}
