using LeadSoft.Common.GlobalDomain.DTOs;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Messages;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Text;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Leads
{
    public partial class LeadService(IDocumentStore ravenDB) : ILeadService
    {
        public async Task<DTOBoolResponse> ImportAsync(IFormFile csvFile, CultureName cultureName, LeadOrigin leadOrigin)
        {
            using Stream fileStream = ReadFileAsUtf8(csvFile.OpenReadStream());

            IAsyncDocumentSession session = ravenDB.OpenAsyncSession(sessionOptions: new() { NoTracking = true });

            Template template = await session.Query<Template>()
                                             .FirstOrDefaultAsync(t => t.Destination == leadOrigin);

            IList<Lead> leads = leadOrigin == LeadOrigin.List2
                                    ? [.. List2.ToLeads(fileStream)]
                                    : [.. List1.ToLeads(fileStream)];

            using var bulkInsert = ravenDB.BulkInsert();

            foreach (Lead lead in leads)
                await bulkInsert.StoreAsync(lead.SetTemplate(template.Id).SetCulture(cultureName));

            return new(true);
        }

        private static MemoryStream ReadFileAsUtf8(Stream fileStream)
        {
            Encoding encoding = Encoding.Default;
            string original = string.Empty;

            using (StreamReader sr = new(fileStream, Encoding.UTF8))
            {
                original = sr.ReadToEnd();
                encoding = sr.CurrentEncoding;
                sr.Close();
            }

            //byte[] utf8Bytes = Encoding.UTF8.GetBytes(original);
            //string utf8String = Encoding.UTF8.GetString(utf8Bytes);
            return new MemoryStream(Encoding.UTF8.GetBytes(original));
        }
    }
}
