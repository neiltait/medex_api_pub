using AutoMapper;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    ///     Extension method to add our profiles to the mapper configuration.
    /// </summary>
    public static class MedicalExaminerProfiles
    {
        /// <summary>
        ///     Add Our Profiles to the mapper configuration.
        /// </summary>
        /// <param name="config">The mapper configuration.</param>
        public static void AddMedicalExaminerProfiles(this IMapperConfigurationExpression config)
        {
            config.AddProfile<ExaminationProfile>();
            config.AddProfile(new PermissionsProfile());
            config.AddProfile(new UsersProfile());
            config.AddProfile<MedicalTeamProfile>();
            config.AddProfile<PatientDetailsProfile>();
            config.AddProfile<CaseBreakdownProfile>();
            config.AddProfile<OtherEventProfile>();
            config.AddProfile<AdmissionEventProfile>();
            config.AddProfile<BereavedDiscussionEventProfile>();
            config.AddProfile<MedicalHistoryEventProfile>();
            config.AddProfile<MeoSummaryEventProfile>();
            config.AddProfile<NewExaminationProfile>();
            config.AddProfile<PreScrutinyEventProfile>();
            config.AddProfile<QapDiscussionEventProfile>();
            config.AddProfile<CaseOutcomeProfile>();
        }
    }
}