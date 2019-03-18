using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    public class AppointmentFinder
    {
        public AppointmentFinder() { }
        public Representative FindAppointment(IEnumerable<Representative> representatives)
        {
            if (representatives == null)
            {
                return null;
            }
            
            return representatives.OrderByDescending(x => x.AppointmentDate)
                .FirstOrDefault(repAppointment => repAppointment.AppointmentDate >= DateTime.Now);
        }
    }
}
