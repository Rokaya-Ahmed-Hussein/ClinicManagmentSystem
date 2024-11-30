using BLL.Interfaces;
using DAL.Data;
using DAL.enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL
{
    public class DoctorScheduleService :IDoctorScheduleService
    {
        private readonly ApplicationDbContext _context; 

        public DoctorScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoctorSchedule>> GetDoctorSchedulesAsync()
        {
            return await _context.DoctorSchedules.Include(ds => ds.Doctor).ToListAsync();
        }

        public async Task<DoctorSchedule> GetDoctorScheduleByIdAsync(int id)
        {
            return await _context.DoctorSchedules
                .Include(ds => ds.Doctor)
                .FirstOrDefaultAsync(ds => ds.DoctorScheduleID == id);
        }



        public async Task AddDoctorScheduleAsync(DoctorSchedule doctorSchedule)
        {
            await _context.DoctorSchedules.AddAsync(doctorSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDoctorScheduleAsync(DoctorSchedule doctorSchedule)
        {
            _context.DoctorSchedules.Update(doctorSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoctorScheduleAsync(int id)
        {
            var schedule = await _context.DoctorSchedules.FindAsync(id);
            if (schedule != null)
            {
                _context.DoctorSchedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DoctorSchedule>> GetSchedulesByDoctorAsync(int doctorId)
        {
            return await _context.DoctorSchedules
                .Where(ds => ds.DoctorID == doctorId)
                .Include(ds => ds.Doctor)
                .ToListAsync();
        }
    }
}
