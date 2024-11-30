using DAL.Data;

namespace Clinic_Managment_System.Viewmodel
{
    public class ClinicOverviewViewModel
    {
        public int TotalDoctors { get; set; }
        public int TotalStaff { get; set; }
        public int TotalAppointments { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<Staff> Staff { get; set; }
    }

}
