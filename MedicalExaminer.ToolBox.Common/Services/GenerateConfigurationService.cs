using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using MedicalExaminer.ToolBox.Common.Dtos.Generate;

namespace MedicalExaminer.ToolBox.Common.Services
{
    public class GenerateConfigurationService
    {
        private readonly ICosmosStore<Location> _locationStore;
        private readonly ICosmosStore<MeUser> _userStore;
        private readonly ICosmosStore<Permission> _permissionStore;

        public GenerateConfigurationService(
            ICosmosStore<Location> locationStore,
            ICosmosStore<MeUser> userStore,
            ICosmosStore<Permission> permissionStore)
        {
            _locationStore = locationStore;
            _userStore = userStore;
            _permissionStore = permissionStore;
        }

        public async Task Generate(GenerateConfiguration configuration)
        {
            await ClearLocations();
            await ClearUsers();

            var nl = await GenerateLocation(null, LocationType.National, 0);

            await GenerateUsers(
                nl,
                configuration.NumberOfServiceOwnersPerNational,
                0,
                0,
                0);

            for (int ri = 0; ri < configuration.NumberOfRegions; ri++)
            {
                var rl = await GenerateLocation(nl, LocationType.Region, ri);

                await GenerateUsers(
                    rl,
                    0,
                    configuration.NumberOfServiceAdministratorsPerRegion,
                    configuration.NumberOfMedicalExaminersPerRegion,
                    configuration.NumberOfMedicalExaminerOfficersPerRegion);

                for (int ti = 0; ti < configuration.NumberOfTrusts; ti++)
                {
                    var tl = await GenerateLocation(rl, LocationType.Trust, ti);

                    await GenerateUsers(
                        tl,
                        0,
                        0,
                        configuration.NumberOfMedicalExaminersPerTrust,
                        configuration.NumberOfMedicalExaminerOfficersPerTrust);

                    for (int si = 0; si < configuration.NumberOfSites; si++)
                    {
                        var sl = await GenerateLocation(tl, LocationType.Site, si);

                        await GenerateUsers(
                            sl,
                            0,
                            0,
                            configuration.NumberOfMedicalExaminersPerSite,
                            configuration.NumberOfMedicalExaminerOfficersPerSite);
                    }
                }
            }
        }

        private async Task ClearLocations()
        {
            await _locationStore.RemoveAsync(l => true);
        }

        private async Task ClearUsers()
        {
            await _permissionStore.RemoveAsync(p => true);
            await _userStore.RemoveAsync(u => true);
        }

        private async Task<Location> GenerateLocation(Location parent, LocationType locationType, int index)
        {
            var location = new Location()
            {
                Name = NameForLocation(locationType, parent, index),
                ParentId = parent?.LocationId,
                Type = locationType,
            };

            var result = await _locationStore.UpsertAsync(location);

            return result.Entity;
        }

        private async Task GenerateUsers(
            Location location,
            int numberOfOwners,
            int numberOfAdministrators,
            int numberOfMedicalExaminers,
            int numberOfMedicalExaminerOfficers)
        {
            await GenerateUsers(location, UserRoles.ServiceOwner, numberOfOwners);
            await GenerateUsers(location, UserRoles.ServiceAdministrator, numberOfAdministrators);
            await GenerateUsers(location, UserRoles.MedicalExaminer, numberOfMedicalExaminers);
            await GenerateUsers(location, UserRoles.MedicalExaminerOfficer, numberOfMedicalExaminerOfficers);
        }

        private async Task GenerateUsers(Location location, UserRoles role, int number)
        {
            foreach (var i in Enumerable.Range(0, number))
            {
                await GenerateUser(location, role, i);
            }
        }

        private async Task<MeUser> GenerateUser(Location location, UserRoles role, int index)
        {
            var id = Guid.NewGuid().ToString();

            var permission = new Permission()
            {
                PermissionId = Guid.NewGuid().ToString(),
                LocationId = location.LocationId,
                UserRole = (int) role,
            };

            await _permissionStore.UpsertAsync(permission);

            var user = new MeUser()
            {
                UserId = id,
                FirstName = $"{role}{index}",
                LastName = $"At-{location.Name}",
                Email = $"{id}@example.com",
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        LocationId = location.LocationId,
                        PermissionId = permission.PermissionId,
                        UserRole = (int)role,
                    }
                }
            };

            var result = await _userStore.UpsertAsync(user);

            return result.Entity;
        }

        private static string NameForLocation(LocationType locationType, Location parent, int index)
        {
            var prefix = parent != null ? $"{parent.Name}-" : string.Empty;

            switch (locationType)
            {
                case LocationType.Site:
                    return $"{prefix}Site{index}";
                case LocationType.Trust:
                    return $"{prefix}Trust{index}";
                case LocationType.Region:
                    return $"{prefix}Region{index}";
                case LocationType.National:
                    return $"{prefix}National{index}";
                default:
                    break;
            }

            return "Unknown";
        }
    }
}
