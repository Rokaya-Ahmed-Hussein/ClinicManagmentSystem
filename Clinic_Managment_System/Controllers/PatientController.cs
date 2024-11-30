using BLL.interfaces;
using BLL.Viewmodel;
using Clinic_Managment_System.Viewmodel;
using DAL.Data;
using DAL.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Managment_System.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var patient = _context.Patients.FirstOrDefault(d => d.ApplicationUserId == user.Id);



            var totalAppointments = await _context.Appointments
                .Where(a => a.PatientID == patient.PatientID)
                .CountAsync();

            var completedAppointments = await _context.Appointments
                .Where(a => a.PatientID == patient.PatientID && a.Status == Status.Completed)
                .CountAsync();

            var missedAppointments = await _context.Appointments
                .Where(a => a.PatientID == patient.PatientID && a.Status == Status.Missed)
                .CountAsync();

            var viewModel = new PatientAppointmentOverviewViewModel
            {
                TotalAppointments = totalAppointments,
                CompletedAppointments = completedAppointments,
                MissedAppointments = missedAppointments
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details()
        {

            var user = await _userManager.GetUserAsync(User);
            var patient = _context.Patients.FirstOrDefault(d => d.ApplicationUserId == user.Id);


            var appointment = _context.Appointments
                .Where(x => x.PatientID == patient.PatientID)
                .Include(x => x.Patient)
                .ToList();
            return View(appointment);
        }


        public IActionResult Appoitment()
        {
            // Create a list of specialties from the enum
            var specialties = Enum.GetValues(typeof(Specialty))
                .Cast<Specialty>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(), 
                    Text = s.ToString() 
                }).ToList();

            // Assign the list to ViewBag
            ViewBag.Specialties = specialties;
            return View();
        }

        [HttpGet]
        public JsonResult GetDoctorsBySpecialty(Specialty specialty)
        {
            var doctors = _context.Doctors
                .Where(s => s.Specialty == specialty)
                .Select(s => new { s.DoctorID, s.Name })
                .ToList();

            return Json(doctors);
        }

        [HttpGet]
        public async Task<JsonResult> GetClincsByDoctor(int doctorId)
        {
            // First, ensure the doctor exists
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null)
            {
               
                return Json(new { success = false, message = "Doctor not found" });
            }

            var clinics = await _context.Clinics
                .Where(c => c.Doctors.Any(d => d.DoctorID == doctorId))
                .Select(c => new
                {
                    c.ClinicID,
                    c.Name,
                })
                .ToListAsync();

            return Json(new { success = true, clinics });
        }



        [HttpGet]
        public async Task<JsonResult> GetDaysByDoctorAndClinic(int clinicId, int doctorId)
        {
            var days = await _context.DoctorSchedules
                .Where(ds => ds.DoctorID == doctorId && ds.Doctor.ClinicID == clinicId)
                .Select(ds => new
                {
                    Day = ds.ScheduleDay.ToString() 
                })
                .Distinct()
                .ToListAsync();

            return Json(days);
        }

        [HttpGet]
        public async Task<JsonResult> GetTimeSlotsByDoctorAndClinicAndDay(int clinicId, int doctorId, string day)
        {
            if (Enum.TryParse(typeof(Days), day, out var dayEnum))
            {
                var timeSlots = await _context.DoctorSchedules
                    .Where(ds => ds.DoctorID == doctorId && ds.Doctor.ClinicID == clinicId && ds.ScheduleDay == (Days)dayEnum)
                    .Select(ds => new
                    {
                        TimeSlot = ds.TimeSlot.ToString(), 
                        Price = ds.Price
                    })
                    .ToListAsync();

                return Json(timeSlots);
            }

            return Json(new List<object>()); 
        }

        [HttpGet]
        public async Task<JsonResult> GetPriceByDoctorAndClinicAndDayAndTimeSlot(int clinicId, int doctorId, string day, string timeSlot)
        {
            if (Enum.TryParse(typeof(Days), day, out var dayEnum) && Enum.TryParse(typeof(TimeSlot), timeSlot, out var timeSlotEnum))
            {
                var price = await _context.DoctorSchedules
                    .Where(ds => ds.DoctorID == doctorId && ds.Doctor.ClinicID == clinicId && ds.ScheduleDay == (Days)dayEnum && ds.TimeSlot == (TimeSlot)timeSlotEnum)
                    .Select(ds => ds.Price)
                    .FirstOrDefaultAsync();

                return Json(price);
            }

            return Json(null); 
        }




        [HttpPost]
        public async Task<IActionResult> AddAppointment(AppointmentViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var patient = _context.Patients.FirstOrDefault(d => d.ApplicationUserId == user.Id);


            // Create a new Appointment entity
            var appointment = new Appointment
            {
                PatientID = patient.PatientID,
                DoctorID = model.SelectedDoctorId,
                ClinicID = model.SelectedClinicId,
                SelectedDay = model.SelectedDay, 
                SelectedTimeSlot = model.SelectedTimeSlot, 
                ReasonForVisit = model.ReasonForVisit,
                Status = Status.Scheduled 
            };

            _context.Appointments.Add(appointment);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");







        }
    }
}
