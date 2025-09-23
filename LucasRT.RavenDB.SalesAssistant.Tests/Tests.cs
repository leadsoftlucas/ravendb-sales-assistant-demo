using LeadSoft.Common.GlobalDomain.DTOs;
using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Leads;
using LucasRT.RavenDB.SalesAssistant.Tests.Fixtures.Databases;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace LucasRT.RavenDB.SalesAssistant.Tests
{
    public partial class Tests(RavenDBFixture fixture) : IClassFixture<RavenDBFixture>
    {
        public readonly ILeadService leadService = new LeadService(fixture.RavenDB);

        [Fact]
        public async Task CSVImport()
        {
            using Stream csvStream = Assembly.GetExecutingAssembly().GetEmbeddedResourceStream($"RavenDB_TDC_SP_2025_Detailed_Scanner_List.csv");
            IFormFile csvFile = new FormFile(csvStream, 0, csvStream.Length, "Data", "RavenDB_TDC_SP_2025_Detailed_Scanner_List.csv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };

            DTOBoolResponse dto = await leadService.ImportAsync(csvFile);

            Assert.True(dto.IsTrue());
        }
    }
}
