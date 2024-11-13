
using AppointmentManagementSystem.Data.Enums;

namespace AppointmentManagementSystem.Data.Models;
public class MassageAppointment(Customer customer, MassageType massageType, EmployeeGender employeeSex, DateTime appointmentDate, string? appointmentNotes = null)
: Appointment(customer, appointmentDate, appointmentNotes)
{
    public MassageType MassageType { get; set; } = massageType;
   public EmployeeGender EmployeeSex { get; set; } = employeeSex;

}