using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Finance
{
    public class FinanceServiceTests : ServiceTestsBase<
        FinanceQuery,
        ExaminationConnectionSettings,
        IEnumerable<MedicalExaminer.Models.Examination>,
        MedicalExaminer.Models.Examination,
        FinanceService>
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ExaminationsQueryExpressionBuilder>();

            var store = CosmosMocker.CreateCosmosStore(GetExamples());
            services.AddTransient<ICosmosStore<MedicalExaminer.Models.Examination>>(s => store.Object);

            base.ConfigureServices(services);
        }

        [Fact]
        public virtual async Task DateRangeReturnsCorrectCount()
        {
            //Arrange
            var dateFrom = new DateTime(2010, 2, 1);
            var dateTo = new DateTime(2010, 2, 27);
            var locationId = "location1";
            var permissedLocation = new string[] { "location1" };

            var financeQuery = new FinanceQuery(dateFrom, dateTo, locationId, permissedLocation);

            //Act
            var results = await Service.Handle(financeQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.Count());
        }

        [Fact]
        public virtual async Task NullParamThrowsError()
        {
            // Act
            Action act = () => Service.Handle(null).GetAwaiter().GetResult();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        protected override MedicalExaminer.Models.Examination[] GetExamples()
        {
            var examination1 = new MedicalExaminer.Models.Examination()
            {
                CreatedAt = new DateTime(2010, 2, 1),
                SiteLocationId = "location1"
            };

            var examination2 = new MedicalExaminer.Models.Examination()
            {
                CreatedAt = new DateTime(2010, 2, 20),
                SiteLocationId = "location2"
            };

            var examination3 = new MedicalExaminer.Models.Examination()
            {
                CreatedAt = new DateTime(2010, 3, 1),
                SiteLocationId = "location1"
            };

            return new MedicalExaminer.Models.Examination[]
            {
                examination1,
                examination2,
                examination3
            };
        }
    }
}
