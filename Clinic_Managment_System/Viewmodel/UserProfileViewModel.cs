using DAL.Data;
using DAL.enums;

namespace Clinic_Managment_System.Viewmodel
{
    public class UserProfileViewModel
    {
        // Common Properties
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }

        // Admin Properties
        public string AdminName { get; set; }

        // Doctor Properties
        public string DoctorName { get; set; }
        public Specialty Specialty { get; set; }
        public int ClinicID { get; set; } 
        public decimal Price { get; set; } 

        // Clinic Properties
        public string ClinicName { get; set; }
        public string Location { get; set; }
        public ICollection<Doctor?> Doctors { get; set; } 

        // Patient Properties
        public string PatientName { get; set; }
        public string NationalID { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string MedicalHistory { get; set; }
    }


}
