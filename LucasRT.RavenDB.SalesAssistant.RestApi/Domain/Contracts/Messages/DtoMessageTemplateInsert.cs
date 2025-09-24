using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
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
        public string Text { get; set; } = string.Empty;
    }
}
