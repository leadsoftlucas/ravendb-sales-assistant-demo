using LeadSoft.Common.GlobalDomain.Entities;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads
{
    public partial class Lead
    {
        public string Id { get; set; } = null;
        public string TemplateId { get; set; } = string.Empty;
        public LeadOrigin Origin { get; set; } = LeadOrigin.Other;
        public CultureName CultureName { get; set; } = CultureName.pt_BR;
        public string Name { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public Emails Emails { get; set; } = new();
        public Gender Gender { get; set; } = Gender.Undefined;
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; } = null;
        public int? Age { get => BirthDate.HasValue ? DateTime.Now.Year - BirthDate.Value.Year - (DateTime.Now < BirthDate.Value.AddYears(DateTime.Now.Year - BirthDate.Value.Year) ? 1 : 0) : null; }
        public IList<UF> State { get; set; } = [];
        public Phones Phones { get; set; } = new();
        public AiKnowledge AiKnowledge { get; set; }

        public Lead SetTemplate(string templateId)
        {
            TemplateId = templateId;
            return this;
        }

        public Lead SetCulture(CultureName cultureName)
        {
            CultureName = cultureName;
            return this;
        }
    }

    public enum LeadOrigin
    {
        List1 = 1,
        List2 = 2,
        Other = 99
    }
}
