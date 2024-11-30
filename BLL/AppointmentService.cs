using BLL.interfaces;
using BLL.Viewmodel;
using DAL.Data;
using DAL.enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
 
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context; 

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

     

        public async Task<int> GetTotalAppointmentsAsync(int doctorId)
        {
            return await _context.Appointments
                                 .Where(a => a.DoctorID == doctorId)
                                 .CountAsync();
        }

        public async Task<int> GetCompletedAppointmentsAsync(int doctorId)
        {
            return await _context.Appointments
                                 .Where(a => a.DoctorID == doctorId && a.Status == Status.Completed)
                                 .CountAsync();
        }

        public async Task<int> GetMissedAppointmentsAsync(int doctorId)
        {
            return await _context.Appointments
                                 .Where(a => a.DoctorID == doctorId && a.Status == Status.Missed)
                                 .CountAsync();
        }



    }

}
