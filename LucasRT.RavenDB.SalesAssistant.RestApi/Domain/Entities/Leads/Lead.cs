using LeadSoft.Common.GlobalDomain.Entities;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads
{
    public partial class Lead
    {
        public string Id { get; set; } = null;
        public string Name { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public Gender Gender { get; set; } = Gender.Other;
        public Emails Emails { get; set; } = new();
        public DateTime BirthDate { get; set; } = new DateTime(1900, 1, 1);
        public int Age { get => DateTime.Now.Year - BirthDate.Year - (DateTime.Now < BirthDate.AddYears(DateTime.Now.Year - BirthDate.Year) ? 1 : 0); }
        public IList<UF> State { get; set; } = [];
        public Phones Phones { get; set; } = new();
    }
}
