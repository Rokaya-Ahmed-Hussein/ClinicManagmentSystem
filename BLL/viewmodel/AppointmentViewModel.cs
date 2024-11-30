using DAL.enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BLL.Viewmodel
{
   public class AppointmentViewModel
{
    public int SelectedDoctorId { get; set; }
    public int SelectedClinicId { get; set; }
    public Days SelectedDay { get; set; }
    public TimeSlot SelectedTimeSlot { get; set; }
    
    [Required]
    public string ReasonForVisit { get; set; } 

    public List<SelectListItem> Clinics { get; set; }
    public List<SelectListItem> Specialties { get; set; }
}


}
