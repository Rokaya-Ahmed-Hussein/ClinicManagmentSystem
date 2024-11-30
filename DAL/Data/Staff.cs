using DAL.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class Staff
    {
        public int StaffID { get; set; } 

        [Required]
        public string Name { get; set; } 

        [Required]
        [EmailAddress]
        public string Email { get; set; } 

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } 

        [Required]
        public StaffRole Role { get; set; } 

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; } 

        public int ClinicID { get; set; } 
        public Clinic Clinic { get; set; } 
    }

}
