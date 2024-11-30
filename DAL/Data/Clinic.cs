using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class Clinic
    {
        public int ClinicID { get; set; } 

        [Required]
        public string Name { get; set; } 

        [Required]
        public string Location { get; set; } 

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } 

        [Required]
        [EmailAddress]
        public string Email { get; set; } 
                                          
        public string ApplicationUserId { get; set; }

        
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public ICollection<Doctor> Doctors { get; set; } 
        public ICollection<Staff> StaffMembers { get; set; } 
        public ICollection<Appointment> Appointments { get; set; } 
    }


}
