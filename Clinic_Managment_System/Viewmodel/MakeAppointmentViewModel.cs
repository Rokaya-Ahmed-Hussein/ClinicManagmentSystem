using DAL.enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic_Managment_System.Viewmodel
{
    public class MakeAppointmentViewModel
    {
        public Specialty SelectedSpecialty { get; set; }
        public int SelectedDoctorId { get; set; }
        public List<SelectListItem> Specialties { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Doctors { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableDays { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> TimeSlots { get; set; } = new List<SelectListItem>();
        public decimal Price { get; set; }
        public string ReasonForVisit { get; set; }
    }

}
