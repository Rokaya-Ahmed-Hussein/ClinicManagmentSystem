using BLL;
using Clinic_Managment_System.Viewmodel;
using DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLL;
using Clinic_Managment_System.Viewmodel;
using DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DAL.enums;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Clinic_Managment_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            ViewBag.Roles = new List<string> { "Admin", "Doctor", "Clinic", "Patient" };
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign the selected role to the user
                    await _userManager.AddToRoleAsync(user, model.Role);

                    // Redirect to the profile completion view based on role
                    return RedirectToAction("CompleteProfile", new { role = model.Role, userId = user.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.Roles = new List<string> { "Admin", "Doctor", "Clinic", "Patient" };
            return View(model);
        }

        // GET: CompleteProfile
        public IActionResult CompleteProfile(string role, string userId)
        {
            switch (role)
            {
                case "Admin":
                    return View("CompleteAdminProfile", new AdminProfileViewModel { ApplicationUserId = userId });
                case "Doctor":
                    ViewBag.Clinics = _context.Clinics
           .Select(c => new SelectListItem
           {
               Value = c.ClinicID.ToString(),
               Text = c.Name
           }).ToList();

                    ViewBag.Specialties = Enum.GetValues(typeof(Specialty))
                        .Cast<Specialty>()
                        .Select(s => new SelectListItem
                        {
                            Value = s.ToString(),
                            Text = s.ToString() // Adjust this if you want to format the text differently
                        }).ToList();
                    return View("CompleteDoctorProfile", new DoctorProfileViewModel { ApplicationUserId = userId });
                case "Clinic":
                    return View("CompleteClinicProfile", new ClinicProfileViewModel { ApplicationUserId = userId });
                case "Patient":
                    return View("CompletePatientProfile", new PatientProfileViewModel { ApplicationUserId = userId });
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompletePatientProfile(PatientProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    NationalID = model.NationalID,
                    Gender = model.Gender,
                    Age = model.Age,
                    MedicalHistory = model.MedicalHistory,
                    ApplicationUserId = model.ApplicationUserId
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteDoctorProfile(DoctorProfileViewModel model)
        {

            
            ViewBag.Clinics = await _context.Clinics
                .Select(c => new SelectListItem
                {
                    Value = c.ClinicID.ToString(),
                    Text = c.Name
                }).ToListAsync();

            ViewBag.Specialties = Enum.GetValues(typeof(Specialty))
                .Cast<Specialty>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = s.ToString()
                }).ToList();

            // Validate ClinicID
            var clinicExists = await _context.Clinics.AnyAsync(c => c.ClinicID == model.ClinicID);
            if (!clinicExists)
            {
                ModelState.AddModelError("ClinicID", "The selected clinic does not exist.");
                return View(model); 
            }

       
                var doctor = new Doctor
                {
                    Name = model.Name,
                    Specialty = model.Specialty,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    ApplicationUserId = model.ApplicationUserId,
                    ClinicID = model.ClinicID 
                };

                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");




        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteClinicProfile(ClinicProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var clinic = new Clinic
                {
                    Name = model.Name,
                    Location = model.Location,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    ApplicationUserId = model.ApplicationUserId
                };

                _context.Clinics.Add(clinic);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteAdminProfile(AdminProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = new Admin
                {
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    ApplicationUserId = model.ApplicationUserId
                };

                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        public async Task<IActionResult> Profile()
        {
            // Get the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId); 

            if (user == null)
            {
                return NotFound(); // User not found
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            // Create a view model to store user profile information
            var profileViewModel = new UserProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = role
            };

            // Fetch additional details based on user role
            if (role == Roles.Admin)
            {
                var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == user.Email);
                if (admin != null)
                {
                    profileViewModel.AdminName = admin.Name;
                }
            }
            else if (role == Roles.Doctor)
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Name == user.FullName);
                if (doctor != null)
                {
                    profileViewModel.DoctorName = doctor.Name;
                    profileViewModel.Specialty = doctor.Specialty;
                    profileViewModel.ClinicID = doctor.ClinicID;
                }
            }
            else if (role == Roles.Clinic)
            {
                var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.Email == user.Email);
                if (clinic != null)
                {
                    profileViewModel.ClinicName = clinic.Name;
                    profileViewModel.Location = clinic.Location;
                    if(clinic.Doctors == null )
                    {
                        profileViewModel.Doctors = new List<Doctor>();
                    }
                    else {
                        profileViewModel.Doctors = clinic.Doctors.ToList();
                    }
                }
            }
            else if (role == Roles.Patient)
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == user.Email);
                if (patient != null)
                {
                    profileViewModel.PatientName = patient.Name;
                    profileViewModel.NationalID = patient.NationalID;
                    profileViewModel.Gender = patient.Gender;
                    profileViewModel.Age = patient.Age;
                    profileViewModel.MedicalHistory = patient.MedicalHistory;
                }
            }

            return View(profileViewModel); // Return the populated view model to the view
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var roles = await _userManager.GetRolesAsync(user);
                    return RedirectToAction("Index", GetControllerNameForRole(roles.FirstOrDefault() ?? "Home"));
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    
            }

            return View(model);
        }
        private string GetControllerNameForRole(string role)
        {
            switch (role)
            {
                case "Admin":
                    return "Admin"; 
                case "Doctor":
                    return "Doctor"; 
                case "Clinic":
                    return "Clinic"; 
                case "Patient":
                    return "Patient"; 
                default:
                    return "Home";
            }
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }


        }
}
