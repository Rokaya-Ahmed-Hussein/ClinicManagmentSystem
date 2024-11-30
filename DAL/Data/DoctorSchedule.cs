using DAL.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class DoctorSchedule
    {
        public int DoctorScheduleID { get; set; } 

        public int DoctorID { get; set; } 
        public Doctor Doctor { get; set; } 

        [Required]
        public Days ScheduleDay { get; set; } 

        [Required]
        public TimeSlot TimeSlot { get; set; } 

        public int MaxPatients { get; set; } 
        public decimal Price { get; set; } 
    }

}
