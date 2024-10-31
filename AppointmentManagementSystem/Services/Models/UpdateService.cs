namespace AppointmentManagementSystem.Services.Models;

public class UpdateService(int id, string updateField, string updateValue)
{
    public int Id { get; set;} = id;
    public string UpdateField { get; set;} = updateField;
    public string UpdateValue { get; set; } = updateValue;
}
