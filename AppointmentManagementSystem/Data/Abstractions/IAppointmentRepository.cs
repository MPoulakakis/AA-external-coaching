using System.Collections.ObjectModel;
using AppointmentManagementSystem.Data.Models;

namespace AppointmentManagementSystem.Data.Abstractions;

public interface IAppointmentRepository
{
    Task<ReadOnlyCollection<Appointment>> GetAppointments();
    Task CreateAppointment(Appointment appointment, int id);
    Task UpdateAppointment(int id, string updateField, string? updateValue,DateTime? updateDateValue, Customer? customer);
    Task DeleteAppointment(int id);
}