using LeadSoft.Common.GlobalDomain.DTOs;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Leads
{
    public interface ILeadService
    {
        Task<DTOBoolResponse> ImportAsync(IFormFile csvFile, CultureName cultureName, LeadOrigin leadOrigin);
    }
}
