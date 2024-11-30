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
    [Authorize(Roles = "Clinic")]
    public class ClinicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public ClinicController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }



        public async Task<IActionResult> Index()
        {
            // Get the current logged-in user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            // Find the clinic associated with the current user
            var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.ApplicationUserId == currentUser.Id);
            if (clinic == null)
            {
                return NotFound("No clinic associated with the current user.");
            }

            var totalDoctors = await _context.Doctors
                                             .Where(d => d.ClinicID == clinic.ClinicID)
                                             .CountAsync();

            var totalStaff = await _context.Staffs
                                           .Where(s => s.ClinicID == clinic.ClinicID)
                                           .CountAsync();

            var totalAppointments = await _context.Appointments
                                                  .Where(a => _context.Doctors
                                                                      .Where(d => d.ClinicID == clinic.ClinicID)
                                                                      .Select(d => d.DoctorID)
                                                                      .Contains(a.DoctorID))
                                                  .CountAsync();

            var model = new ClinicOverviewViewModel
            {
                TotalDoctors = totalDoctors,
                TotalStaff = totalStaff,
                TotalAppointments = totalAppointments
            };

            return View(model);
        }


        // Staff Management Actions
        public async Task<IActionResult> Staff()
        {
            // Get the current logged-in user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.ApplicationUserId == currentUser.Id);
            if (clinic == null)
            {
                return NotFound("No clinic associated with the current user.");
            }

            // Filter staff by the current clinic's ClinicID
            var staff = await _context.Staffs
                                      .Where(s => s.ClinicID == clinic.ClinicID)
                                      .ToListAsync();

            return View(staff);
        }


        public IActionResult AddStaff()
        {
            ViewBag.Roles = Enum.GetValues(typeof(StaffRole))
                                 .Cast<StaffRole>()
                                 .Select(e => new SelectListItem
                                 {
                                     Value = e.ToString(),
                                     Text = e.ToString() 
                                 });
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddStaff(Staff staff)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                ModelState.AddModelError("", "User not found.");
                ViewBag.Roles = GetRoleSelectList();
                return View(staff);
            }

            var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.ApplicationUserId == currentUser.Id);
            if (clinic == null)
            {
                ModelState.AddModelError("", "No clinic associated with the current user.");
                ViewBag.Roles = GetRoleSelectList();
                return View(staff);
            }

            // Set the ClinicID for the new staff member
            staff.ClinicID = clinic.ClinicID;

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Staff));
        }




        private IEnumerable<SelectListItem> GetRoleSelectList()
        {
            return Enum.GetValues(typeof(StaffRole))
                       .Cast<StaffRole>()
                       .Select(e => new SelectListItem
                       {
                           Value = e.ToString(),
                           Text = e.ToString()
                       });
        }



      
        public async Task<IActionResult> EditStaff(int id)
        {
            // Find the staff member by ID
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            ViewBag.Roles = Enum.GetValues(typeof(StaffRole))
                                 .Cast<StaffRole>()
                                 .Select(e => new SelectListItem
                                 {
                                     Value = e.ToString(),
                                     Text = e.ToString()
                                 });

            ViewData["Title"] = "Edit Staff";
            return View(staff);
        }



        [HttpPost]
        public async Task<IActionResult> EditStaff(Staff staff)
        {

            var existingStaff = await _context.Staffs.FindAsync(staff.StaffID);

            if (existingStaff == null)
            {
                return NotFound(); // If not found, return a 404 error
            }

            existingStaff.Name = staff.Name;
            existingStaff.Email = staff.Email;
            existingStaff.PhoneNumber = staff.PhoneNumber;
            existingStaff.Role = staff.Role;
            existingStaff.Salary = staff.Salary;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Staff));


        }

        

        public async Task<IActionResult> DeleteStaff(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.ApplicationUserId == currentUser.Id);
            if (clinic == null)
            {
                return NotFound("No clinic associated with the current user.");
            }

            var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.StaffID == id && s.ClinicID == clinic.ClinicID);
            if (staff != null)
            {
                _context.Staffs.Remove(staff);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Staff));
        }


        public async Task<IActionResult> Doctors()
        {
            // Get the current logged-in user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            // Find the clinic associated with the current user
            var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.ApplicationUserId == currentUser.Id);
            if (clinic == null)
            {
                return NotFound("No clinic associated with the current user.");
            }

            // Retrieve doctors associated with the current clinic
            var doctors = await _context.Doctors.Where(d => d.ClinicID == clinic.ClinicID).ToListAsync();

            return View(doctors);
        }


        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Doctors));
        }
    }
}
