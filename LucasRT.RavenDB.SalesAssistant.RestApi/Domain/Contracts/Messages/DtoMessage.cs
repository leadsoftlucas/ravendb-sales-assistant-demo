using System.Runtime.Serialization;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Contracts.Messages
{
    [Serializable]
    [DataContract]
    public partial class DtoMessage
    {
        [DataMember]
        public string Recipient { get; set; } = string.Empty;

        [DataMember]
        public string Subject { get; set; } = string.Empty;

        [DataMember]
        public string Body { get; set; } = string.Empty;
    }
}
