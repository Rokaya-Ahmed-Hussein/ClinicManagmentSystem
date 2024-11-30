using System.ComponentModel.DataAnnotations;

namespace Clinic_Managment_System.Viewmodel
{
    public class AdminProfileViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

       
        public string ApplicationUserId { get; set; }
    }


}
