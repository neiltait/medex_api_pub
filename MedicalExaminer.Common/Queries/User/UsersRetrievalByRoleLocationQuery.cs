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
        /// <param name="roles">Roles to query. Or none.</param>
        public UsersRetrievalByRoleLocationQuery(IEnumerable<string> locations, IEnumerable<UserRoles> roles)
        {
            Locations = locations;
            Roles = roles;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRetrievalByRoleLocationQuery"/> class.
        /// </summary>
        /// <param name="locations">The locations.</param>
        /// <param name="role">The role.</param>
        public UsersRetrievalByRoleLocationQuery(IEnumerable<string> locations, UserRoles role)
            : this(locations, new[] { role })
        {
        }

        /// <summary>
        /// List of locations ids.
        /// </summary>
        public IEnumerable<string> Locations { get; }

        /// <summary>
        /// Role to query on.
        /// </summary>
        public IEnumerable<UserRoles> Roles { get; }
    }
}
