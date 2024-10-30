using System.Collections.ObjectModel;
using AppointmentManagementSystem.Data.Abstractions;
using AppointmentManagementSystem.Data.Models;
using Spectre.Console;

namespace AppointmentManagementSystem.Data.Repositories
{
    public class InMemoryAppointmentRepository(List<Appointment>? initialData = null) : IAppointmentManagementSystem
    {
         private readonly List<Appointment> _appointments = initialData ?? [];

        public Task CreateAppointment(Appointment appointment, int id)
        {
            var foundCustomer = _appointments.Find(appointment => appointment.Customer.Id == id);
            if (foundCustomer != null)
            {
                _appointments.Add(appointment);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAppointment(int id)
        {
            var foundAppointment = _appointments.Find(x => x.Id == id);
            
            if (foundAppointment is not null)
            {
                _appointments.Remove(foundAppointment);
            }
            else
            {
                AnsiConsole.Markup("[red]Appointment not Found[/]\n");
            }
            return Task.CompletedTask;
        }

        public Task<ReadOnlyCollection<Appointment>> GetAppointments() => Task.FromResult(_appointments.AsReadOnly());

        public Task UpdateAppointment(int id, string updateField, string? updateValue = null, DateTime? updateDateValue = null, Customer? customer = null)
        {
            
            var foundAppointment = _appointments.Find(x => x.Id == id);
            if (foundAppointment is not null)
            {
                switch (updateField) {
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
                        break;;
                }
            }
            else
            {
                AnsiConsole.Markup("[red]Appointment not Found[/]\n");
            }
            return Task.CompletedTask;
        }
    }
}