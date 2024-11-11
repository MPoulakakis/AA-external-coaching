using System.Collections.ObjectModel;
using AppointmentManagementSystem.Data.Models;

namespace AppointmentManagementSystem.Data.Abstractions;

public interface IAppointmentRepository
{
    Task<ReadOnlyCollection<Appointment>> GetAppointments();
    Task CreateAppointment(Appointment appointment);
    Task UpdateAppointment(Appointment appointment);
    Task<bool> DeleteAppointment(int id);
    Task<Appointment> AppointmentExists(int id);
}