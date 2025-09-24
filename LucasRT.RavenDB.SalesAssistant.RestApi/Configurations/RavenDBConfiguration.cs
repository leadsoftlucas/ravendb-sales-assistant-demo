using LeadSoft.Common.Library.EnvUtils;
using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain;
using Newtonsoft.Json;
using Raven.Client.Json.Serialization.NewtonsoftJson;
using Raven.DependencyInjection;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Configurations
{
    public static class RavenDBConfiguration
    {

        private static X509Certificate2 GetRavenDBCertificate(IConfiguration configuration)
        {
            X509Certificate2? cert = null;

            if (configuration["RavenSettings:ResourceName"].IsSomething())
                cert = Assembly.GetExecutingAssembly()
                               .GetEmbeddedX509Certificate($"{configuration["RavenSettings:ResourceName"]}", EnvUtil.Get(EnvConstant.RavenDBPwd));


            return cert ?? throw new OperationCanceledException("Certificate not found!");
        }

        public static void AddRavenDB(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddRavenDbDocStore(options =>
            {
                options.Certificate = GetRavenDBCertificate(configuration);
                options.BeforeInitializeDocStore = docStore => docStore.Conventions.Serialization = new NewtonsoftJsonSerializationConventions
                {
                    CustomizeJsonSerializer = serializer => serializer.NullValueHandling = NullValueHandling.Ignore,
                };
            });
            service.AddRavenDbAsyncSession();
        }
    }
}
