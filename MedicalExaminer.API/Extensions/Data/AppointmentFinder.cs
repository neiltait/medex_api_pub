using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    /// class that finds the next available appointment of an examination
    /// </summary>
    public class AppointmentFinder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentFinder"/> class.
        /// </summary>
        public AppointmentFinder() { }

        /// <summary>
        /// finds the next available appointment in the patients details
        /// </summary>
        /// <param name="representatives"></param>
        /// <returns></returns>
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
