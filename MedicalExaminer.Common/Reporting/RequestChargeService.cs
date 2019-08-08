using System.Collections.Generic;

namespace MedicalExaminer.Common.Reporting
{
    public class RequestChargeService
    {
        public class RequestCharge
        {
            public string Request { get; set; }

            public double Charge { get; set; }
        }

        public IList<RequestCharge> RequestCharges { get; } = new List<RequestCharge>();
    }
}
