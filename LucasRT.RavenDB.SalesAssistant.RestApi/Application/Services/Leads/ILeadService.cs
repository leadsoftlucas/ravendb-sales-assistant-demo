using LeadSoft.Common.GlobalDomain.DTOs;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Leads
{
    public interface ILeadService
    {
        Task<DTOBoolResponse> ImportAsync(IFormFile csvFile);
    }
}
