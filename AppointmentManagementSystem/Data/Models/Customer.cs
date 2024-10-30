namespace AppointmentManagementSystem.Data.Models;

public class Customer(string name, string email, string phone)
{
    private static int _incrementalId = 0;
    public int Id { get; private set; } = ++ _incrementalId;
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Phone { get; set; } = phone;
}
