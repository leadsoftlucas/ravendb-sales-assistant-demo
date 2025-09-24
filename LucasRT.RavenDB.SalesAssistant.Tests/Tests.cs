using LeadSoft.Common.GlobalDomain.DTOs;
using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Leads;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using LucasRT.RavenDB.SalesAssistant.Tests.Fixtures.Databases;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace LucasRT.RavenDB.SalesAssistant.Tests
{
    public partial class Tests(RavenDBFixture fixture) : IClassFixture<RavenDBFixture>
    {
        public readonly ILeadService leadService = new LeadService(fixture.RavenDB);

        [Fact]
        public async Task List1Import()
        {
            using Stream csvStream = Assembly.GetExecutingAssembly().GetEmbeddedResourceStream($"List1.csv");
            IFormFile csvFile = new FormFile(csvStream, 0, csvStream.Length, "Data", "List1.csv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };

            DTOBoolResponse dto = await leadService.ImportAsync(csvFile, CultureName.pt_BR, LeadOrigin.List1);

            Assert.True(dto.IsTrue());
        }

        [Fact]
        public async Task List2Import()
        {
            using Stream csvStream = Assembly.GetExecutingAssembly().GetEmbeddedResourceStream($"List2.csv");
            IFormFile csvFile = new FormFile(csvStream, 0, csvStream.Length, "Data", "List2.csv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };

            DTOBoolResponse dto = await leadService.ImportAsync(csvFile, CultureName.pt_BR, LeadOrigin.List2);

            Assert.True(dto.IsTrue());
        }
    }
}
