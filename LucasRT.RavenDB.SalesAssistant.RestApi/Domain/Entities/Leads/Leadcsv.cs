using LeadSoft.Common.GlobalDomain.Entities;
using LeadSoft.Common.Library;
using LeadSoft.Common.Library.Extensions;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads
{
    public partial class Leadcsv
    {
        public string Name { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Addresses { get; set; } = string.Empty;
        public string AddressesTypes { get; set; } = string.Empty;
        public string Phones { get; set; } = string.Empty;
        public string PhoneTypes { get; set; } = string.Empty;
        public string OtherEmails { get; set; } = string.Empty;
        public string OtherEmailsTypes { get; set; } = string.Empty;

        public static IEnumerable<Leadcsv> FromCsv(Stream aStream)
        {
            IList<string[]> lines = Util.ReadCSVStreamToTheEnd(aStream, true, string.Empty, false);

            foreach (string[] line in lines)
            {
                yield return new()
                {
                    Name = line[0].Trim(),
                    DocumentNumber = line[1].Trim(),
                    Gender = line[2].Trim()[..1] ?? "?",
                    Email = line[3].Trim(),
                    BirthDate = line[4].Trim(),
                    State = line[5].Trim(),
                    Phones = line[6].Trim(),
                    PhoneTypes = line[7].Trim(),
                    OtherEmails = line[8].Trim(),
                    OtherEmailsTypes = line[9].Trim()
                };
            }
        }

        public static implicit operator Lead(Leadcsv leadCsv)
        {
            if (leadCsv is null)
                return null;

            Lead lead = new()
            {
                Name = leadCsv.Name.ToTitleCase(),
                DocumentNumber = leadCsv.DocumentNumber.FormatCPF(),
                BirthDate = DateTime.TryParse(leadCsv.BirthDate, out DateTime birthDate) ? birthDate : new DateTime(1900, 1, 1),
                Gender = leadCsv.Gender switch
                {
                    "M" => LeadSoft.Common.Library.Enumerators.Enums.Gender.Male,
                    "F" => LeadSoft.Common.Library.Enumerators.Enums.Gender.Female,
                    _ => LeadSoft.Common.Library.Enumerators.Enums.Gender.Other
                }
            };

            if (leadCsv.State.IsSomething())
                foreach (UF uf in leadCsv.State.Split(",").Select(uf => GetByValueString<UF>(uf)))
                    lead.State.Add(uf);

            lead.Emails.SetPrimary(new(Enums.ContactType.Personal, leadCsv.Email));

            if (leadCsv.OtherEmails.IsSomething())
            {
                int idx = 0;
                foreach (string email in leadCsv.OtherEmails.Split(","))
                {
                    string emailType = leadCsv.OtherEmailsTypes.Split(",").ElementAtOrDefault(idx)?.Trim().ToUpper() ?? "PERSONAL";

                    Enums.ContactType contactType = emailType.ToUpper() switch
                    {
                        "WORK" => Enums.ContactType.Professional,
                        "MOBILE" => Enums.ContactType.Mobile,
                        "HOME" => Enums.ContactType.Home,
                        "PERSONAL" => Enums.ContactType.Personal,
                        "CORPORATE" => Enums.ContactType.Commercial,
                        _ => Enums.ContactType.Other
                    };

                    if (email.IsSomething())
                        lead.Emails.Add(new(contactType, email), false);

                    idx++;
                }
            }

            if (leadCsv.Phones.IsSomething())
            {
                int idx = 0;
                foreach (string phone in leadCsv.Phones.Split(","))
                {
                    string phoneType = leadCsv.PhoneTypes.Split(",").ElementAtOrDefault(idx)?.Trim().ToUpper() ?? "Personal";

                    Enums.ContactType contactType = phoneType.ToUpper() switch
                    {
                        "WORK" => Enums.ContactType.Professional,
                        "MOBILE" => Enums.ContactType.Mobile,
                        "HOME" => Enums.ContactType.Home,
                        _ => Enums.ContactType.Other
                    };

                    if (phone.IsSomething())
                        lead.Phones.Add(new(contactType, phone[2..], false), false);
                    idx++;
                }
            }

            return lead;
        }

        public static IEnumerable<Leadcsv> ToLeads(Stream stream)
        {
            foreach (Leadcsv leadcsv in Leadcsv.FromCsv(stream))
                yield return leadcsv;
        }
    }
}
