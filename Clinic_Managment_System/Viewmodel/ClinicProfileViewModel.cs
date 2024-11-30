using System.ComponentModel.DataAnnotations;

namespace Clinic_Managment_System.Viewmodel
{
    public class ClinicProfileViewModel
    {
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
    }


}
