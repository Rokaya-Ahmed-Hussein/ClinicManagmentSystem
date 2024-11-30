using DAL.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IDoctorScheduleService
    {
        Task<IEnumerable<DoctorSchedule>> GetDoctorSchedulesAsync(); 
        Task<DoctorSchedule> GetDoctorScheduleByIdAsync(int id); 
        Task AddDoctorScheduleAsync(DoctorSchedule doctorSchedule); 
        Task UpdateDoctorScheduleAsync(DoctorSchedule doctorSchedule); 
        Task DeleteDoctorScheduleAsync(int id); 
        Task<IEnumerable<DoctorSchedule>> GetSchedulesByDoctorAsync(int doctorId); 
    }
}
