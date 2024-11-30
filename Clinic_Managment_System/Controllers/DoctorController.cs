using BLL.Interfaces;
using BLL.Viewmodel;
using DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using BLL.interfaces;
using Clinic_Managment_System.Viewmodel;
using DAL.enums;
using Microsoft.EntityFrameworkCore;
using BLL;
using System.Security.Claims;

namespace Clinic_Managment_System.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorScheduleService _doctorScheduleService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorController(BLL.Interfaces.IDoctorScheduleService doctorScheduleService, IAppointmentService appointmentService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _appointmentService = appointmentService;
            _context = context;
            _userManager = userManager;
            _doctorScheduleService = doctorScheduleService;

        }

  
        public async Task<IActionResult> Index()
        {
            // Get the current logged-in user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            // Find the doctor associated with the current user
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == currentUser.Id);
            if (doctor == null)
            {
                return NotFound("No doctor profile associated with the current user.");
            }

            // Get total, completed, and missed appointments for the current doctor
            var totalAppointments = await _appointmentService.GetTotalAppointmentsAsync(doctor.DoctorID);
            var completedAppointments = await _appointmentService.GetCompletedAppointmentsAsync(doctor.DoctorID);
            var missedAppointments = await _appointmentService.GetMissedAppointmentsAsync(doctor.DoctorID);

            var model = new DoctorDashboardViewModel
            {
                TotalAppointments = totalAppointments,
                CompletedAppointments = completedAppointments,
                MissedAppointments = missedAppointments
            };

            return View(model);
        }


        // View Doctor's Schedule
        public async Task<IActionResult> ManageSchedule()
        {
            var user = await _userManager.GetUserAsync(User);
            var doctor = _context.Doctors.FirstOrDefault(d => d.ApplicationUserId == user.Id);

            if (doctor == null)
            {
                return NotFound();
            }

            var schedules = await _context.DoctorSchedules
                .Where(ds => ds.DoctorID == doctor.DoctorID)
                .ToListAsync();

            return View(schedules);
        }

        // Create Schedule
        public IActionResult CreateSchedule()
        {
       
            ViewBag.Days = Enum.GetValues(typeof(Days)).Cast<Days>().ToDictionary(day => day.ToString(), day => day.ToString());
            ViewBag.TimeSlots = Enum.GetValues(typeof(TimeSlot)).Cast<TimeSlot>().ToDictionary(slot => slot.ToString(), slot => slot.ToString());

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule(DoctorSchedule model)
        {
       
                // Assuming you have a method to get the current doctor's ID
                var user = await _userManager.GetUserAsync(User);
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == user.Id);

                if (doctor != null)
                {
                    model.DoctorID = doctor.DoctorID; // Set the DoctorID from the current user
                    _context.DoctorSchedules.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ManageSchedule");
                }
                ModelState.AddModelError("", "Doctor not found."); 
      

            // If the model state is invalid, repopulate the ViewBag and return to the view
            ViewBag.Days = Enum.GetValues(typeof(Days)).Cast<Days>().ToDictionary(day => day.ToString(), day => day.ToString());
            ViewBag.TimeSlots = Enum.GetValues(typeof(TimeSlot)).Cast<TimeSlot>().ToDictionary(slot => slot.ToString(), slot => slot.ToString());

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditSchedule(int id)
        {
            // Populate ViewBag with Days and TimeSlots
            ViewBag.Days = Enum.GetValues(typeof(Days)).Cast<Days>().ToDictionary(day => day.ToString(), day => day.ToString());
            ViewBag.TimeSlots = Enum.GetValues(typeof(TimeSlot)).Cast<TimeSlot>().ToDictionary(slot => slot.ToString(), slot => slot.ToString());

            var schedule = await _context.DoctorSchedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
          
            return View(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> EditSchedule(DoctorSchedule schedule)
        {
            // Populate ViewBag with Days and TimeSlots
            ViewBag.Days = Enum.GetValues(typeof(Days)).Cast<Days>().ToDictionary(day => day.ToString(), day => day.ToString());
            ViewBag.TimeSlots = Enum.GetValues(typeof(TimeSlot)).Cast<TimeSlot>().ToDictionary(slot => slot.ToString(), slot => slot.ToString());

            
            var existingSchedule = await _context.DoctorSchedules.FindAsync(schedule.DoctorScheduleID);
            if (existingSchedule == null)
            {
                return NotFound();
            }

            
            existingSchedule.ScheduleDay = schedule.ScheduleDay;
            existingSchedule.TimeSlot = schedule.TimeSlot;
            existingSchedule.MaxPatients = schedule.MaxPatients;
            existingSchedule.Price = schedule.Price;

            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageSchedule));
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.DoctorSchedules.FindAsync(id);
            if (schedule != null)
            {
                _context.DoctorSchedules.Remove(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageSchedule"); 
            }
            return RedirectToAction("ManageSchedule"); 
        }

        public async Task<IActionResult> ManageAppointments()
        {
            var user = await _userManager.GetUserAsync(User);
            var doctor = _context.Doctors.FirstOrDefault(d => d.ApplicationUserId == user.Id);

            var appointments = await _context.Appointments
                .Where(a => a.DoctorID == doctor.DoctorID)
                .ToListAsync();

            var statuses = Enum.GetValues(typeof(Status))
                               .Cast<Status>()
                               .ToDictionary(status => status.ToString(), status => status.ToString());

            ViewBag.Statuses = statuses;


            return View(appointments);
        }

    
        [HttpPost]
        public async Task<IActionResult> ChangeAppointmentStatus(int appointmentId, Status newStatus)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment != null)
            {
                appointment.Status = newStatus;
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageAppointments));
        }

    }
}
