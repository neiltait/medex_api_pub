using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmonaut;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Models;
using MedicalExaminer.ToolBox.Common.Dtos;

namespace MedicalExaminer.ToolBox.Common.Services
{
    public class GetLocationTreeService
    {
        private readonly ICosmosStore<Location> _locationStore;
        private readonly ICosmosStore<MeUser> _userStore;

        public GetLocationTreeService(ICosmosStore<Location> locationStore, ICosmosStore<MeUser> userStore)
        {
            _locationStore = locationStore;
            _userStore = userStore;
        }

        private LocationNode PopulateTree(
            LocationNode node, 
            IList<Location> source,
            IList<MeUser> sourceUsers,
            IList<UserItem> allUsers)
        {
            var sourceChildren = source.Where(l => l.ParentId == node.LocationId).ToList();

            var sourceUser = sourceUsers
                .Where(u => sourceChildren
                    .Any(l => u.Permissions != null && u.Permissions.Any(p => p.LocationId == l.LocationId)))
                .Select(u => u.UserId);

            node.Users = sourceUser
                .Select(u => allUsers.First(a => a.UserId == u))
                .ToList();

            node.Children = sourceChildren.Select(l => PopulateTree(new LocationNode()
            {
                LocationId = l.LocationId,
                IsMeOffice = l.IsMeOffice,
                Parent = node,
                Name = l.Name,
            }, source, sourceUsers, allUsers));

            return node;
        }

        private IList<Location> GetPath(IEnumerable<Location> children)
        {
            var todo = children.ToList();
            var result = new List<Location>();

            while(todo.Any())
            {
                result.AddRange(todo);

                var parents = todo.Select(l => l.ParentId).Distinct().ToList();

                var existingIds = result.Select(r => r.LocationId).ToList();

                var next = _locationStore
                    .Query()
                    .Where(l => parents.Contains(l.LocationId) && !existingIds.Contains(l.LocationId))
                    .ToList();

                todo = next;
            }

            return result.Distinct().ToList();
        }

        public LocationResponse GetTree()
        {
            var sourceUsers = _userStore.Query().ToList();

            var permissedLocations = sourceUsers
                .Where(u => u.Permissions != null)
                .SelectMany(u => u.Permissions.Select(p => p.LocationId)).Distinct().ToList();
            
            var offices = _locationStore
                .Query()
                .Where(l => l.IsMeOffice // Is office
                            || permissedLocations.Contains(l.LocationId)) // Or user has it assigned via permission
                .ToList();

            var allLocations = GetPath(offices);


            var allUsers = sourceUsers.Select(u => new UserItem()
            {
                UserId = u.UserId,
                FullName = u.FullName()
            }).ToList();

            var result = new LocationResponse();
            var roots = new List<LocationNode>();
            result.Roots = roots;

            var sourceRoots = allLocations.Where(l => l.ParentId == null).ToList();

            foreach(var sourceRoot in sourceRoots)
            {
                var rootNode = new LocationNode()
                {
                    LocationId = sourceRoot.LocationId,
                    Name = sourceRoot.Name,
                };

                PopulateTree(rootNode, allLocations, sourceUsers, allUsers);

                roots.Add(rootNode);
            }

            /*result.Locations = allLocations.Select(l => new LocationNode()
            {
                LocationId = l.LocationId,
                Name = l.Name
            }).ToList();*/

            return result;
        }
    }
}
