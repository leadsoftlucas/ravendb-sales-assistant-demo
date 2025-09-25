using LeadSoft.Common.Library;
using LeadSoft.Common.Library.Extensions;
using static LeadSoft.Common.GlobalDomain.Entities.Enums;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads
{
    public partial class List1
    {
        public string Company { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public static IEnumerable<List1> FromCsv(Stream aStream)
        {
            IList<string[]> lines = Util.ReadCSVStreamToTheEnd(aStream, true, string.Empty, false);

            foreach (string[] line in lines)
            {
                yield return new()
                {
                    Company = line[0].Trim(),
                    Name = line[1].Trim(),
                    Email = line[2].Trim()
                };
            }
        }

        public static implicit operator Lead(List1 leadSoft)
        {
            if (leadSoft is null)
                return null;

            Lead lead = new()
            {
                Origin = LeadOrigin.List1,
                Name = leadSoft.Name.ToTitleCase(),
                Company = leadSoft.Company.ToTitleCase(),
            };

            lead.Emails.SetPrimary(new(ContactType.Personal, leadSoft.Email));

            return lead;
        }

        public static IEnumerable<List1> ToLeads(Stream stream)
        {
            foreach (List1 list1 in FromCsv(stream))
                yield return list1;
        }
    }
}
