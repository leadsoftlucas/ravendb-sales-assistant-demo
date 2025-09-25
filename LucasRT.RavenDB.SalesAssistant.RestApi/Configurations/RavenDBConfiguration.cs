using LeadSoft.Common.Library.EnvUtils;
using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads.Indexes;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Messages;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.AI;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Json.Serialization.NewtonsoftJson;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
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

        #region [ Static Indexes ]

        /// <summary>
        /// Deploys static indexes to the RavenDB document store asynchronously.
        /// </summary>
        /// <remarks>This method retrieves the <see cref="IDocumentStore"/> from the provided <paramref
        /// name="service"/> collection and deploys a predefined list of static indexes to it. Ensure that the RavenDB
        /// document store is properly configured in the service collection before calling this method.</remarks>
        /// <param name="service">The service collection used to retrieve the RavenDB document store.</param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException">Thrown if the RavenDB document store is not found in the service collection.</exception>
        public static async Task SetRavenDB_StaticIndexsAsync(this IServiceCollection service)
        {
            IDocumentStore ravenDB = service.BuildServiceProvider().GetService<IDocumentStore>() ?? throw new OperationCanceledException("RavenDB Document Store not found!");

            List<AbstractIndexCreationTask> indexesToDeploy =
            [
              new Index_LeadsMessages()
            ];

            await IndexCreation.CreateIndexesAsync(indexesToDeploy, ravenDB);
        }

        #endregion

        #region [ AI Tasks ]

        public const string _RavenDB_Database = "SalesAssistant";
        public const string _RavenDB_AI_ConnectionString = "OpenAI-Generative";
        public const string _RavenDB_AI_ConnectionString_Identifier = "openai-generative";

        /// <summary>
        /// Configures the OpenAI connection string for the RavenDB document store within the specified service
        /// collection.
        /// </summary>
        /// <remarks>This method retrieves the RavenDB document store from the service collection and
        /// checks if the OpenAI connection string is already configured. If not, it creates and adds a new OpenAI
        /// connection string using environment variables for the API key, endpoint, and model. This operation is
        /// performed asynchronously.</remarks>
        /// <param name="service">The service collection to configure with the OpenAI connection string.</param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException">Thrown if the RavenDB document store is not found in the service collection.</exception>
        public static void SetRavenDBOpenAIGenerativeConnectionString(this IServiceCollection service)
        {
            IDocumentStore ravenDB = service.BuildServiceProvider().GetService<IDocumentStore>() ?? throw new OperationCanceledException("RavenDB Document Store not found!");

            //DatabaseRecordWithEtag record = ravenDB.Maintenance.Server.Send(new GetDatabaseRecordOperation(_RavenDB_Database));
            //if (record.AiConnectionStrings.Any(cs => cs.Key.Equals(_RavenDB_AI_ConnectionString)))
            //    return;

            AiConnectionString connectionString = new()
            {
                Name = _RavenDB_AI_ConnectionString,
                Identifier = _RavenDB_AI_ConnectionString_Identifier,
                ModelType = AiModelType.Chat,
                OpenAiSettings = new OpenAiSettings(
                    apiKey: EnvUtil.Get(EnvConstant.OpenAIApiKey),
                    endpoint: EnvUtil.Get(EnvConstant.OpenAIApiUrl),
                    model: EnvUtil.Get(EnvConstant.OpenAIGenerativeModel))
                {
                    EmbeddingsMaxConcurrentBatches = 4
                }
            };

            PutConnectionStringOperation<AiConnectionString> operation = new(connectionString);
            ravenDB.Maintenance.Send(operation);
            //PutConnectionStringResult putConnectionStringResult = ravenDB.Maintenance.Send(operation);
        }

        public const string _RavenDB_AI_Task_LeadKnowledge = "LeadKnowledge";
        public const string _RavenDB_AI_Task_LeadKnowledge_Identifier = "leadknowledge";

        /// <summary>
        /// Configures and registers a RavenDB AI task for lead knowledge processing within the specified service
        /// collection.
        /// </summary>
        /// <remarks>This method sets up a RavenDB AI task named "LeadKnowledge" if it is not already
        /// configured. It requires a valid RavenDB document store and AI connection string to be present in the service
        /// collection. The task analyzes lead data to extract insights based on specified rules and
        /// transformations.</remarks>
        /// <param name="service">The service collection to which the RavenDB AI task is added.</param>
        /// <exception cref="OperationCanceledException">Thrown if the RavenDB document store or the required AI connection string is not found in the service
        /// collection.</exception>
        public static void SetRavenDBAITask_LeadKnowledge(this IServiceCollection service)
        {
            IDocumentStore ravenDB = service.BuildServiceProvider().GetService<IDocumentStore>() ?? throw new OperationCanceledException("RavenDB Document Store not found!");

            DatabaseRecordWithEtag record = ravenDB.Maintenance.Server.Send(new GetDatabaseRecordOperation(_RavenDB_Database));

            if (!record.AiConnectionStrings.Any(cs => cs.Key.Equals(_RavenDB_AI_ConnectionString)))
                throw new OperationCanceledException("RavenDB Document Store not found!");

            GenAiConfiguration config = new()
            {
                Name = _RavenDB_AI_Task_LeadKnowledge,
                Identifier = _RavenDB_AI_Task_LeadKnowledge_Identifier,
                ConnectionStringName = _RavenDB_AI_ConnectionString,
                Disabled = false,
                MaxConcurrency = 4,
                Collection = nameof(Lead).GetPlural(),
                GenAiTransformation = new GenAiTransformation
                {
                    Script = @"
                        ai.genContext({
                            Name: this.Name,
                            Company: this.Company,
                            Gender: this.Gender,
                            Age: this.Age,
                            Emails: this.Emails,
                            CultureName: this.CultureName
                        })
                    "
                },
                Prompt = @"You are an experienced SDR and wants to understand the best way to classify and know your leads to contact them, so analyze their data and extract the information you can gather from it. Use strict real-world data.
                            Rules:
                            - The gender and generation may affect the way you choose the words.
                            - If no company is specified, explore it's e-mail list to find whitch it works at checking the domains or the e-mail address name if it's a company, but ignore default/generic e-mail services provider (@hotmail.com, @icloud.com, @gmail.com or others). If you are not sure if the he works company exists, leave it blank.".Trim(),

                SampleObject = AiKnowledge.GetAIFieldsInstructions().ToJson(),
                UpdateScript = @"this.AiKnowledge = $output;"
            };

            if (record.GenAis.Any(cs => cs.Name.Equals(_RavenDB_AI_Task_LeadKnowledge)))
            {
                GenAiConfiguration gen = record.GenAis.Single(cs => cs.Name.Equals(_RavenDB_AI_Task_LeadKnowledge));
                ravenDB.Maintenance.Send(new UpdateGenAiOperation(gen.TaskId, config));
            }
            else
                ravenDB.Maintenance.Send(new AddGenAiOperation(config));

            //AddGenAiOperationResult addAiIntegrationTaskResult = ravenDB.Maintenance.Send(GenAiOperation);
        }

        public const string _RavenDB_AI_Task_CreateMessages = "CreateMessages";
        public const string _RavenDB_AI_Task_CreateMessages_Identifier = "createmessages";

        /// <summary>
        /// Configures a RavenDB AI task for creating messages within the service collection.
        /// </summary>
        /// <remarks>This method sets up a RavenDB AI task named <c>CreateMessages</c> if it does not
        /// already exist. It requires a valid RavenDB document store and AI connection string to be present in the
        /// service collection. If the necessary RavenDB components are not found, an <see
        /// cref="OperationCanceledException"/> is thrown.</remarks>
        /// <param name="service">The service collection to which the RavenDB AI task is added.</param>
        /// <exception cref="OperationCanceledException">Thrown if the RavenDB document store or the required AI connection string is not found in the service
        /// collection.</exception>
        public static void SetRavenDBAITask_CreateMessages(this IServiceCollection service)
        {
            IDocumentStore ravenDB = service.BuildServiceProvider().GetService<IDocumentStore>() ?? throw new OperationCanceledException("RavenDB Document Store not found!");

            DatabaseRecordWithEtag record = ravenDB.Maintenance.Server.Send(new GetDatabaseRecordOperation(_RavenDB_Database));

            if (!record.AiConnectionStrings.Any(cs => cs.Key.Equals(_RavenDB_AI_ConnectionString)))
                throw new OperationCanceledException("RavenDB Document Store not found!");

            GenAiConfiguration config = new()
            {
                Name = _RavenDB_AI_Task_CreateMessages,
                Identifier = _RavenDB_AI_Task_CreateMessages_Identifier,
                ConnectionStringName = _RavenDB_AI_ConnectionString,
                Disabled = false,
                MaxConcurrency = 4,
                Collection = nameof(Message).GetPlural(),
                GenAiTransformation = new GenAiTransformation
                {
                    Script = @"
                       ai.genContext({    
                            Template: this.Template.Text,
                            Company: this.AiKnowledge.Company,
                            FriendlyName: this.AiKnowledge.FriendlyName,
                            PersonalEmails: this.AiKnowledge.PersonalEmails,
                            ProfessionalEmail: this.AiKnowledge.ProfessionalEmail,
                            SpokenLanguage: this.AiKnowledge.SpokenLanguage,
                            VoiceTarget: this.AiKnowledge.VoiceTarget
                        })
                    "
                },
                Prompt = @"You are an experienced Account Executive and received the leads enriched from your SDR with a suggested e-mail template.
                           Use the Lead information to refine and do small adjustments to personificate the template to the e-mail you'll send them.
                           Depending on Lead's information, you'll have professional and/or personal contexts to work on, maybe one of them won't exist and that's ok.
                           The e-mail body should be written on lead's language, culture and fit the voice target.".Trim(),

                SampleObject = AIEmailSuggestions.GetAIFieldsInstructions().ToJson(),
                UpdateScript = @"this.AIEmailSuggestions = $output"
            };

            if (record.GenAis.Any(cs => cs.Name.Equals(_RavenDB_AI_Task_CreateMessages)))
            {
                GenAiConfiguration gen = record.GenAis.Single(cs => cs.Name.Equals(_RavenDB_AI_Task_CreateMessages));
                ravenDB.Maintenance.Send(new UpdateGenAiOperation(gen.TaskId, config));
            }
            else
                ravenDB.Maintenance.Send(new AddGenAiOperation(config));

            //AddGenAiOperationResult addAiIntegrationTaskResult = ravenDB.Maintenance.Send(GenAiOperation);
        }

        #endregion
    }
}
