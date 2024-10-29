namespace AppointmentManagementSystem.Data.Models;

public class Customer(int id, string name, string email, string phone)
{
    public int Id { get; private set; } = id;
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Phone { get; set; } = phone;
}
