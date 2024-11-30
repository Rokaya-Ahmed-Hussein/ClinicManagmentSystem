using DAL.enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Clinic_Managment_System.Viewmodel
{
    public class DoctorProfileViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Specialty Specialty { get; set; } 

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

       
        public string ApplicationUserId { get; set; }

       
        public int ClinicID { get; set; } 
        public IEnumerable<SelectListItem> Clinics { get; set; }
    }


}
