using DAL.Data;

namespace Clinic_Managment_System.Viewmodel
{
    public class ClinicViewModel
    {
        public IEnumerable<Staff> Staff { get; set; }
        public IEnumerable<Doctor> Doctors { get; set; }
    }

}
