using AppointmentManagementSystem.Data.Models;
using System.Collections.ObjectModel;

namespace AppointmentManagementSystem.Data.Abstractions;

public interface ICustomerRepository
{
    Task<ReadOnlyCollection<Customer>> GetCustomers();
    Task CreateCustomer(Customer customer);
    Task UpdateCustomer(Customer customer);
    Task DeleteCustomer(int id);
    Task<Customer> CustomerExists(int id);
}
