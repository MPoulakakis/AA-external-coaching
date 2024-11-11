namespace AppointmentManagementSystem.Data.Models;
public class MassageAppointment(MassageType massageType, EmployeeSex employeeSex )
{
   public MassageType MassageType { get; set; } = massageType;
   public EmployeeSex EmployeeSex { get; set; } = employeeSex;

}