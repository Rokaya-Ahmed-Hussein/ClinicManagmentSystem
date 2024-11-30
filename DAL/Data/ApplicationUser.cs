using Microsoft.AspNetCore.Identity;

namespace DAL.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } 

       
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Clinic> Clinics { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
    }
}
