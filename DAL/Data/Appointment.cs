using DAL.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class Appointment
    {
        public int AppointmentID { get; set; } 

        public int PatientID { get; set; } 
        public Patient Patient { get; set; } 

        public int DoctorID { get; set; } 
        public Doctor Doctor { get; set; } 

        public int ClinicID { get; set; } 
        public Clinic Clinic { get; set; } 

        public Days SelectedDay { get; set; } 
        public TimeSlot SelectedTimeSlot { get; set; } 

        [Required]
        public string ReasonForVisit { get; set; } 

        public Status Status { get; set; } 
    }

}
