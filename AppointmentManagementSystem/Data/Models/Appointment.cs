namespace AppointmentManagementSystem.Data.Models;

public class Appointment(Customer customer, DateTime appointmentDate, string? appointmentNotes = null)
{
    private static int _incrementalId = 0;
    public int Id { get; private set; } = ++ _incrementalId;
    //public string ServiceType { get; set; } = serviceType;
    public DateTime AppointmentDate { get; set; } = appointmentDate;
    public string? AppointmentNotes { get; set; } = appointmentNotes;
    public Customer Customer { get; set; } = customer;
}
