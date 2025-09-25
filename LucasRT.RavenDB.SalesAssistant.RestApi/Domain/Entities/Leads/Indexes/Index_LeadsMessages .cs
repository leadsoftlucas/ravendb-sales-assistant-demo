using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Messages;
using Raven.Client.Documents.Indexes;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads.Indexes
{
    /// <summary>
    /// Represents an index creation task for the "LeadsMessages" index in a RavenDB database.
    /// </summary>
    /// <remarks>This class defines the mapping and reducing logic for indexing lead messages. It maps
    /// documents from the "Leads" collection, associates them with their corresponding templates, and groups the
    /// results by lead ID.</remarks>
    public class Index_LeadsMessages : AbstractIndexCreationTask
    {
        /// <summary>
        /// Gets the name of the index used for storing lead messages.
        /// </summary>
        public override string IndexName => "LeadsMessages";

        /// <summary>
        /// Creates an index definition for processing lead documents with associated templates.
        /// </summary>
        /// <remarks>This method constructs an index definition that maps lead documents to a structure
        /// containing lead information and associated template details. The index uses a reduce function to group
        /// results by lead ID, ensuring that each lead's data is aggregated correctly. The output is reduced to a
        /// collection named after the plural form of the <see cref="Message"/> class.</remarks>
        /// <returns>An <see cref="IndexDefinition"/> object configured to index lead documents with template associations, using
        /// Lucene as the search engine.</returns>
        public override IndexDefinition CreateIndexDefinition()
            => new()
            {
                Maps =
                {
                    @"from lead in docs.Leads
                        let template = LoadDocument(lead.TemplateId, ""Templates"")
                        select new
                        {
                            LeadId = lead.Id,
                            Origin = lead.Origin,
                            AiKnowledge = lead.AiKnowledge,
                            Template = new
                            {
                                template.Id,
                                template.Destination,
                                template.Text
                            }
                    }"
                },
                Reduce = @"from result in results
                            group result by result.LeadId into g
                            select new {
                                LeadId = g.First().LeadId,
                                Origin = g.First().Origin,
                                AiKnowledge = g.First().AiKnowledge,
                                Template = g.First().Template
                            }",
                OutputReduceToCollection = nameof(Message).GetPlural(),
                Configuration = new IndexConfiguration
                {
                    { "Indexing.Static.SearchEngineType",  Raven.Client.Documents.Indexes.SearchEngineType.Lucene.ToString() }
                }
            };
    }
}
