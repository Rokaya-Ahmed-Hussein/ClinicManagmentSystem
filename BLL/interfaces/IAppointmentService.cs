using BLL.Viewmodel;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.interfaces
{
    public interface IAppointmentService
    {


        Task<int> GetTotalAppointmentsAsync(int doctorId);
        Task<int> GetCompletedAppointmentsAsync(int doctorId);
        Task<int> GetMissedAppointmentsAsync(int doctorId);
    }

}
