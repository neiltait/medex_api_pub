using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;

namespace MedicalExaminer.Common.Services.CaseBreakdown
{
    public class DiscussionOutcomesUpdateService : QueryHandler<NullQuery, bool>
    {
        public DiscussionOutcomesUpdateService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings) : base(databaseAccess, connectionSettings)
        {
        }

        public async override Task<bool> Handle(NullQuery param)
        {
            var examinations = await DatabaseAccess.GetItemsAsync<Models.Examination>(ConnectionSettings, x => true);
            foreach (var examination in examinations)
            {
                if (examination.CaseBreakdown.QapDiscussion.Latest != null)
                {
                    if (examination.CaseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen)
                    {
                        examination.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome = Models.Enums.QapDiscussionOutcome.DiscussionUnableToHappen;
                    }

                    foreach (var item in examination.CaseBreakdown.QapDiscussion.History)
                    {
                        if (item.DiscussionUnableHappen)
                        {
                            item.QapDiscussionOutcome = Models.Enums.QapDiscussionOutcome.DiscussionUnableToHappen;
                        }
                    }
                }

                if (examination.CaseBreakdown.BereavedDiscussion.Latest != null)
                {
                    if (examination.CaseBreakdown.BereavedDiscussion.Latest.DiscussionUnableHappen)
                    {
                        examination.CaseBreakdown.BereavedDiscussion.Latest.BereavedDiscussionOutcome = Models.Enums.BereavedDiscussionOutcome.DiscussionUnableToHappen;
                    }

                    foreach (var item in examination.CaseBreakdown.BereavedDiscussion.History)
                    {
                        if (item.DiscussionUnableHappen)
                        {
                            item.BereavedDiscussionOutcome = Models.Enums.BereavedDiscussionOutcome.DiscussionUnableToHappen;
                        }
                    }
                }

                await DatabaseAccess.UpdateItemAsync(ConnectionSettings, examination);
            }

            return true;
        }
    }
}
