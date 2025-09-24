using LeadSoft.Common.Library.Extensions;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Json.Serialization.NewtonsoftJson;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace LucasRT.RavenDB.SalesAssistant.Tests.Fixtures.Databases
{
    public class RavenDBFixture : IDisposable
    {
        /// <summary>
        /// Obtém o repositório de documentos usado para gerenciar dados relacionados ao editor.
        /// </summary>
        public IDocumentStore RavenDB { get; private set; }

        public RavenDBFixture()
        {
            Stream publisher_Local_Stream = Assembly.GetExecutingAssembly().GetEmbeddedResourceStream($"LucasSales.pfx");
            X509Certificate2 RavenDB_Development = X509CertificateLoader.LoadPkcs12Collection(publisher_Local_Stream.ToBytes(), "LucasHR", X509KeyStorageFlags.DefaultKeySet).First();

            RavenDB = new DocumentStore
            {
                Urls = ["https://a.ravenchildai.development.run/"],
                Database = "SalesAssistant",
                Certificate = RavenDB_Development,
                Conventions =
                {
                    Serialization   = new NewtonsoftJsonSerializationConventions
                    {
                        CustomizeJsonSerializer = serializer => serializer.NullValueHandling = NullValueHandling.Ignore,
                    }
                }
            }.Initialize();
        }

        public void Dispose()
        {
            RavenDB.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
