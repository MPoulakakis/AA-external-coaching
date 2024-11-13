using System.Collections.ObjectModel;
using AppointmentManagementSystem.Data.Models;

namespace AppointmentManagementSystem.Data.Abstractions;

public interface IAppointmentRepository<T> where T : Appointment
{
    Task<ReadOnlyCollection<T>> GetAppointments();
    Task CreateAppointment(T appointment);
    Task UpdateAppointment(T appointment);
    Task<bool> DeleteAppointment(int id);
    Task<T> AppointmentExists(int id);
}