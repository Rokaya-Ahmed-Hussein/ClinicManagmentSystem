using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class Patient
    {
        public int PatientID { get; set; } 

        [Required]
        public string Name { get; set; } 

        [Required]
        [EmailAddress]
        public string Email { get; set; } 

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } 

        [Required]
        public string NationalID { get; set; } 

        [Required]
        public string Gender { get; set; } 

        [Required]
        [Range(0, 120)]
        public int Age { get; set; } 

        public string MedicalHistory { get; set; } 

        public ICollection<Appointment?> Appointments { get; set; } 

       
        public string ApplicationUserId { get; set; }

        
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

}
