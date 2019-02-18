using Medical_Examiner_API;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ME_API_tests.Persistance
{
    public class ExaminationPersistanceFake : IExaminationPersistence
    {
        private List<Examination> examinations;

        public ExaminationPersistanceFake()
        {
            examinations = new List<Examination>();

            Examination ex1 = new Examination();
            Examination ex2 = new Examination();
            Examination ex3 = new Examination();
            ex1.DateOfBirth = new DateTime(2010, 2, 1);
            ex2.DateOfBirth = new DateTime(1998, 6, 23);
            ex3.DateOfBirth = new DateTime(1964, 1, 30);

            ex1.DateOfDeath = new DateTime(2012, 3, 4, 13, 22, 00);
            ex2.DateOfBirth = new DateTime(2017, 4, 17, 23, 1, 00);
            ex3.DateOfBirth = new DateTime(2018, 11, 29, 15, 15, 00);

            ex1.Completed = false;
            ex2.Completed = false;
            ex3.Completed = false;

            ex1.ExaminationId = "aaaaa";
            ex2.ExaminationId = "bbbbb";
            ex3.ExaminationId = "12345zz";

            ex1.FullName = "Robert Bobert";
            ex2.FullName = "Louise Cheese";
            ex3.FullName = "Crowbar Jones";

            ex1.CreatedAt = DateTime.Now;
            ex2.CreatedAt = DateTime.Now;
            ex3.CreatedAt = DateTime.Now;

            ex1.ModifiedAt = DateTime.Now;
            ex2.ModifiedAt = DateTime.Now;
            ex3.ModifiedAt = DateTime.Now;

            examinations.Add(ex1);
            examinations.Add(ex2);
            examinations.Add(ex3);
        }


        public async Task<Examination> GetExaminationAsync(string examinationId)
        {
            foreach (Examination examination in examinations)
            {
                if (examination.ExaminationId == examinationId)
                {
                    return await Task.FromResult(examination);
                }
            }

            throw new ArgumentException("Invalid Argument");
        }

        public async Task<IEnumerable<Examination>> GetExaminationsAsync()
        {
            return await Task.FromResult(examinations);
        }

        public async Task<bool> SaveExaminationAsync(Examination examination)
        {
            return await Task.FromResult(true);
        }
    }
}
