using LeadSoft.Common.GlobalDomain.DTOs;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using Raven.Client.Documents;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Leads
{
    public partial class LeadService(IDocumentStore ravenDB) : ILeadService
    {
        public async Task<DTOBoolResponse> ImportAsync(IFormFile csvFile)
        {
            IList<Lead> leads = [.. Leadcsv.ToLeads(csvFile.OpenReadStream())];

            using var bulkInsert = ravenDB.BulkInsert();

            foreach (Lead item in leads)
                await bulkInsert.StoreAsync(item);

            return new(true);
        }
    }
}
