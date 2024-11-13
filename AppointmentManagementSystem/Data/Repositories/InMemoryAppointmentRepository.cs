using System.Collections.ObjectModel;
using AppointmentManagementSystem.Data.Abstractions;
using AppointmentManagementSystem.Data.Models;

namespace AppointmentManagementSystem.Data.Repositories
{
    public class InMemoryAppointmentRepository<T>() : IAppointmentRepository<T> where T : Appointment
    {
        private readonly List<T> _massageAppointments = InitialData.GetInitialAppointments<T>();//[];  //InitialData.GetInitialAppointments<T>();
        public Task CreateAppointment(T appointment)
        {
            if (appointment is not null)
            {
                _massageAppointments.Add(appointment);
            }
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAppointment(int id)
        {
            bool isDeleted = false;
            var foundAppointment = _massageAppointments.Find(x => x.Id == id);
            isDeleted = foundAppointment is not null && _massageAppointments.Remove(foundAppointment);
            return Task.FromResult(isDeleted);
        }

        public Task<ReadOnlyCollection<T>> GetAppointments() => Task.FromResult(_massageAppointments.AsReadOnly());

        public Task UpdateAppointment(T appointment)
        {
            var foundAppointment = _massageAppointments.Find(x => x.Id == appointment.Id);
            if (foundAppointment is not null)
            {
                //foundAppointment.ServiceType = appointment.ServiceType;
                foundAppointment.AppointmentDate = appointment.AppointmentDate;
                foundAppointment.AppointmentNotes = appointment.AppointmentNotes;
            }
            return Task.CompletedTask;
        }

        public Task<T> AppointmentExists(int id)
        {
            var foundAppointment = _massageAppointments.Find(x => x.Id == id);
            if (foundAppointment is not null)
                return Task.FromResult(foundAppointment);
            else
                return Task.FromResult<T>(null);
        }

    }
}