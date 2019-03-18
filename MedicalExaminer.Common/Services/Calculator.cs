using System;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services
{
    public static class Calculator
    {

        public static int CalculateUrgencyScore(IExamination examination)
        {
            if (examination == null)
            {
                throw new ArgumentNullException(nameof(examination));
            }
            var score = 0;
            if (examination.ChildPriority)
            {
                score = score + 1;
            }
            if (examination.CoronerPriority)
            {
                score = score + 1;
            }
            if (examination.CulturalPriority)
            {
                score = score + 1;
            }
            if (examination.FaithPriority)
            {
                score = score + 1;
            }
            if (examination.OtherPriority)
            {
                score = score + 1;
            }
            return score;
        }
    }
}
