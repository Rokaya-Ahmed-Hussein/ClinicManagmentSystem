using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class Admin
    {
        public int AdminID { get; set; } 

        [Required]
        public string Name { get; set; } 

        [Required]
        [EmailAddress]
        public string Email { get; set; } 

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } 

        [Required]
        public string Password { get; set; } 

        public ICollection<Clinic> Clinics { get; set; } 
        public ICollection<Patient> Patients { get; set; } 
        public ICollection<Doctor> Doctors { get; set; } 
                                                         
        public string ApplicationUserId { get; set; }

        
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
