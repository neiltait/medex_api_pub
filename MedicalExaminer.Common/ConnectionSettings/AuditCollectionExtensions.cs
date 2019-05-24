using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.ConnectionSettings
{
    /// <summary>
    /// Audit Collection Extensions.
    /// </summary>
    public static class AuditCollectionExtensions
    {
        /// <summary>
        /// Gets the Audit collection name from the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>The audit collection name.</returns>
        public static string AuditCollection(this string collection)
        {
            return $"{collection}-Audit";
        }
    }
}
