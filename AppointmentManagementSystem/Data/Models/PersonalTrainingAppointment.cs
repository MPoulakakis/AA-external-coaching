using AppointmentManagementSystem.Data.Enums;

namespace AppointmentManagementSystem.Data.Models;
public class PersonalTrainingAppointment(Customer customer, trainingDuration duration, DateTime appointmentDate,string? focusedTraining = null ,string? injuriesComments = null, string? appointmentNotes = null )
: Appointment(customer, appointmentDate, appointmentNotes)
{
    public trainingDuration Duration { get; set;} = duration;
    public string? FocusedTraining { get; set; } = focusedTraining;
    public string? InjuriesComments { get; set; } = injuriesComments;
}