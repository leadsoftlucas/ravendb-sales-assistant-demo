using LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Messages
{
    public partial class Template
    {
        public string Id { get; set; } = null;
        public LeadOrigin Destination { get; set; } = LeadOrigin.Other;
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Represents a sample template for generating personalized messages.
        /// </summary>
        /// <remarks>This template is designed to be used for creating messages that include placeholders
        /// for  personalizing content, such as the recipient's first name, industry, and company.</remarks>
        public static Template SampleTemplate = new()
        {
            Destination = LeadOrigin.Other,
            Text = "Hi {FirstName},\n\nI came across your profile and was impressed by your experience in {Industry}. I would love to connect and learn more about your work at {Company}, then understand how RavenDB could create value to your business.\n\nBest regards,\n{YourName}"
        };
    }
}
