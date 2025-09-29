using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Contracts.Messages
{
    [Serializable]
    [DataContract]
    public partial class DtoMessageResponse
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string LeadId { get; set; }

        [DataMember]
        public string LeadName { get; set; }

        [DataMember]
        public LeadOrigin LeadOrigin { get; set; }

        [DataMember]
        public string CultureName { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DtoMessage Professional { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DtoMessage Personal { get; set; }
    }
}
