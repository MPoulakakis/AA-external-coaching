using System.Collections.ObjectModel;
using AppointmentManagementSystem.Data.Abstractions;
using AppointmentManagementSystem.Data.Models;
using Spectre.Console;

namespace AppointmentManagementSystem.Data.Repositories
{
    public class InMemoryAppointmentRepository(List<Appointment>? initialData = null) : IAppointmentRepository
    {
         private readonly List<Appointment> _appointments = initialData ?? [];

        public Task CreateAppointment(Appointment appointment)
        {
            if (appointment is not null)
            {
                _appointments.Add(appointment);
            }
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAppointment(int id)
        {
            bool isDeleted = false;
            var foundAppointment = _appointments.Find(x => x.Id == id);
            isDeleted = foundAppointment is not null && _appointments.Remove(foundAppointment);
            return Task.FromResult(isDeleted);
        }

        public Task<ReadOnlyCollection<Appointment>> GetAppointments() => Task.FromResult(_appointments.AsReadOnly());

        public Task UpdateAppointment(Appointment appointment)
        {
            var foundAppointment = _appointments.Find(x => x.Id == appointment.Id);
            if (foundAppointment is not null)
            {
                foundAppointment.ServiceType = appointment.ServiceType;
                foundAppointment.AppointmentDate = appointment.AppointmentDate;
                foundAppointment.AppointmentNotes = appointment.AppointmentNotes;
            }
            return Task.CompletedTask;
        }

        public Task<Appointment> AppointmentExists(int id)
        {
            var foundAppointment = _appointments.Find(x => x.Id == id);
            if (foundAppointment is not null)
                return Task.FromResult(foundAppointment);
            else
                return Task.FromResult<Appointment>(null);
        }
    }
}