using System.ComponentModel;

namespace LucasRT.RavenDB.SalesAssistant.RestApi.Domain.Entities.Leads
{
    public static partial class Enums
    {
        /// <summary>
        /// Specifies the origin of a lead in the system.
        /// </summary>
        /// <remarks>The <see cref="LeadOrigin"/> enumeration is used to categorize the source from which
        /// a lead was acquired. It includes predefined sources such as <see cref="List1"/> and <see cref="List2"/>, as
        /// well as an <see cref="Other"/> option for unspecified sources.</remarks>
        public enum LeadOrigin
        {
            /// <summary>
            /// List 1 - Professional
            /// </summary>
            [Description("List 1 - Professional")]
            List1 = 1,
            /// <summary>
            /// List 2 - Personal
            /// </summary>
            [Description("List 2 - Personal")]
            List2 = 2,
            /// <summary>
            /// Represents a value that is not categorized under any specific predefined category.
            /// </summary>
            /// <remarks>This enumeration value is used to indicate a category that does not fit into
            /// the standard set of categories.</remarks>
            [Description("Other")]
            Other = 99
        }
    }
}
