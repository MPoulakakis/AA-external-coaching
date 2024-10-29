namespace AppointmentManagementSystem.Data.Models;

public class Appointment(Customer customer, string serviceType, DateTime appointmentDate, string? appointmentNotes = null)
{
    public string ServiceType { get; private set; } = serviceType;
    public DateTime AppointmentDate { get; private set; } = appointmentDate;
    public string? AppointmentNotes { get; private set; } = appointmentNotes;
    public Customer Customer { get; private set; } = customer;
}
