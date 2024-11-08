using System.Collections.ObjectModel;
using AppointmentManagementSystem.Data.Models;

namespace AppointmentManagementSystem.Data.Abstractions;

public interface IAppointmentRepository
{
    Task<ReadOnlyCollection<Appointment>> GetAppointments();
    Task CreateAppointment(Appointment appointment);
    Task<bool> UpdateAppointment(int id, string updateField, string? updateValue,DateTime? updateDateValue, Customer? customer);
    Task<bool> DeleteAppointment(int id);
    Task<bool> AppointmentExists(int id);
}