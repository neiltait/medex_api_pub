using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;

namespace MedicalExaminer.Common.Services.CaseBreakdown
{
    public class DiscussionOutcomesUpdateService : QueryHandler<ExaminationsRetrievalQuery, IEnumerable<Models.Examination>>
    {
        public DiscussionOutcomesUpdateService(IDatabaseAccess databaseAccess, IConnectionSettings connectionSettings) : base(databaseAccess, connectionSettings)
        {
        }

        public override Task<IEnumerable<Models.Examination>> Handle(ExaminationsRetrievalQuery param)
        {
            IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Models.Examination>> instance = serviceProvider.GetService<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Models.Examination>>>();
            var strings = new string[] { "377e5b2d-f858-4398-a51c-1892973b6537" };
            var t = instance.Handle(new ExaminationsRetrievalQuery(strings, null, string.Empty, null, 1, 1000, string.Empty, true)).Result;
            foreach (var v in t)
            {
                if (v.CaseBreakdown.QapDiscussion.Latest != null)
                {
                    if (v.CaseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen)
                    {
                        v.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome = MedicalExaminer.Models.Enums.QapDiscussionOutcome.DiscussionUnableToHappen;
                    }

                    foreach (var item in v.CaseBreakdown.QapDiscussion.History)
                    {
                        if (item.DiscussionUnableHappen)
                        {
                            item.QapDiscussionOutcome = MedicalExaminer.Models.Enums.QapDiscussionOutcome.DiscussionUnableToHappen;
                        }
                    }
                }

                if (v.CaseBreakdown.BereavedDiscussion.Latest != null)
                {
                    if (v.CaseBreakdown.BereavedDiscussion.Latest.DiscussionUnableHappen)
                    {
                        v.CaseBreakdown.BereavedDiscussion.Latest.BereavedDiscussionOutcome = MedicalExaminer.Models.Enums.BereavedDiscussionOutcome.DiscussionUnableToHappen;
                    }

                    foreach (var item in v.CaseBreakdown.BereavedDiscussion.History)
                    {
                        if (item.DiscussionUnableHappen)
                        {
                            item.BereavedDiscussionOutcome = MedicalExaminer.Models.Enums.BereavedDiscussionOutcome.DiscussionUnableToHappen;
                        }
                    }
                }
            }
        }
    }
}
