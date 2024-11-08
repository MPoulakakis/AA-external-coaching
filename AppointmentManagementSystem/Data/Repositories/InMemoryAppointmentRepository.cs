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

        public Task<bool> UpdateAppointment(int id, string updateField, string? updateValue = null, DateTime? updateDateValue = null, Customer? customer = null)
        {
            var foundAppointment = _appointments.Find(x => x.Id == id);
            bool isUpdated = false;
            if (foundAppointment is not null)
            {
                switch (updateField) 
                {
                    case "Customer":
                        foundAppointment.Customer = customer;
                        break;
                    case "Service Type":
                        foundAppointment.ServiceType = updateValue;
                        break;
                    case "Appointment Date":
                        foundAppointment.AppointmentDate = updateDateValue ?? default;
                        break;
                    case "Appointment Notes":
                        foundAppointment.AppointmentNotes = updateValue;
                        break;
                }
                isUpdated = true;
                return Task.FromResult(isUpdated);
            }
            return Task.FromResult(isUpdated);
        }

        public Task<bool> AppointmentExists(int id)
        {
            var foundAppointment = _appointments.Find(x => x.Id == id);
            bool appointmentExists = foundAppointment is not null;
            return Task.FromResult(appointmentExists);
        }
    }
}