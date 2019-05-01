using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// Users Retrieval by Role Location Query.
    /// </summary>
    public class UsersRetrievalByRoleLocationQuery : IQuery<IEnumerable<MeUser>>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UsersRetrievalByRoleLocationQuery"/>.
        /// </summary>
        /// <param name="locations">Locations to query on.</param>
        /// <param name="role">Role to query.</param>
        public UsersRetrievalByRoleLocationQuery(IEnumerable<string> locations, UserRoles role)
        {
            Locations = locations;
            Role = role;
        }

        /// <summary>
        /// List of locations ids.
        /// </summary>
        public IEnumerable<string> Locations { get; }

        /// <summary>
        /// Role to query on.
        /// </summary>
        public UserRoles Role { get; }
    }
}
