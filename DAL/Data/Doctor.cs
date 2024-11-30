using DAL.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class Doctor
    {
        public int DoctorID { get; set; } 

        [Required]
        public string Name { get; set; } 

        [Required]
        public Specialty Specialty { get; set; } 

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
                                                 
        public string ApplicationUserId { get; set; }

        
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int ClinicID { get; set; }
        public Clinic? Clinic { get; set; }

        public ICollection<DoctorSchedule?> DoctorSchedules { get; set; } 
        public ICollection<Appointment?> Appointments { get; set; }

    }
}
