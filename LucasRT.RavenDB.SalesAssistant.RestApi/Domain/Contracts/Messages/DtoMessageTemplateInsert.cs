using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Contracts.Messages
{
    [Serializable]
    [DataContract]
    public class DtoMessageTemplateInsert
    {
        [DataMember]
        public LeadOrigin Destination { get; set; } = LeadOrigin.Other;

        [DataMember]
        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(4000)]
        [DefaultValue("Hi {FirstName},\n\nI came across your profile and was impressed by your experience in {Industry}. I would love to connect and learn more about your work at {Company}, then understand how RavenDB could create value to your business.\n\nBest regards,\n{YourName}")]
        public string Text { get; set; } = string.Empty;
    }
}
