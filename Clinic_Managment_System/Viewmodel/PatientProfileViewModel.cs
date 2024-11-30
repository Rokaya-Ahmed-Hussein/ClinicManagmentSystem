using System.ComponentModel.DataAnnotations;

namespace Clinic_Managment_System.Viewmodel
{
    public class PatientProfileViewModel
    {
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

       
        public string ApplicationUserId { get; set; }
    }


}
