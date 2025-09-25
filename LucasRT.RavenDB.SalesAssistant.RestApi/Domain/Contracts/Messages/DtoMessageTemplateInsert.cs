using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Contracts.Messages
{
    /// <summary>
    /// Represents a data transfer object for inserting a message template.
    /// </summary>
    /// <remarks>This class is used to define the structure of a message template that can be personalized
    /// with specific placeholders. It includes properties for specifying the destination based on the lead's origin and
    /// the text content of the message template.</remarks>
    [Serializable]
    [DataContract]
    public class DtoMessageTemplateInsert
    {
        /// <summary>
        /// Gets or sets the destination message based on origin List of the lead.
        /// </summary>
        [DataMember]
        public LeadOrigin Destination { get; set; } = LeadOrigin.Other;

        /// <summary>
        /// Gets or sets the text content of the message template.
        /// </summary>
        /// <remarks>The text is required and must not exceed 4000 characters. It is intended to be used
        /// as a template for personalized messages, with placeholders such as {FirstName}, {Industry}, {Company}, and
        /// {YourName} that can be replaced with actual values.</remarks>
        [DataMember]
        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(4000)]
        [DefaultValue("Hi {FirstName},\n\nI came across your profile and was impressed by your experience in {Industry}. I would love to connect and learn more about your work at {Company}, then understand how RavenDB could create value to your business.\n\nBest regards,\n{YourName}")]
        public string Text { get; set; } = string.Empty;
    }
}
