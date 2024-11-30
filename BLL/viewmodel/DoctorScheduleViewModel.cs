using DAL.enums;

namespace BLL.Viewmodel
{
    public class DoctorScheduleViewModel
    {
        public int DoctorScheduleID { get; set; }
        public int DoctorID { get; set; }
        public Days ScheduleDay { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public int MaxPatients { get; set; }
        public decimal Price { get; set; }
    }

}
