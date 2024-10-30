using AppointmentManagementSystem.Data.Models;
using System.Collections.ObjectModel;

namespace AppointmentManagementSystem.Data.Abstractions;

public interface ICustomerRepository
{
    Task<ReadOnlyCollection<Customer>> GetCustomers();
    Task CreateCustomer(Customer customer);
    Task UpdateCustomer(int id, string updateField, string updateValue);
    Task DeleteCustomer(int id);
    Task<Customer> CustomerExists(int id);
}
