using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Tests.Persistence
{
    public class ExaminationPersistenceFake : IExaminationPersistence
    {
        private readonly List<Examination> _examinations;

        public ExaminationPersistenceFake()
        {
            _examinations = new List<Examination>();

            var ex1 = new Examination();
            var ex2 = new Examination();
            var ex3 = new Examination();
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

            _examinations.Add(ex1);
            _examinations.Add(ex2);
            _examinations.Add(ex3);
        }


        public async Task<Examination> GetExaminationAsync(string examinationId)
        {
            foreach (var examination in _examinations)
                if (examination.ExaminationId == examinationId)
                    return await Task.FromResult(examination);

            throw new ArgumentException("Invalid Argument");
        }

        public async Task<IEnumerable<Examination>> GetExaminationsAsync()
        {
            return await Task.FromResult(_examinations);
        }

        public Task<Guid> CreateExaminationAsync(Examination examinationItem)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveExaminationAsync(Examination examination)
        {
            return await Task.FromResult(true);
        }
    }
}