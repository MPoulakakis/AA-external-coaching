using AppointmentManagementSystem.Data.Abstractions;
using AppointmentManagementSystem.Data.Models;
using Spectre.Console;
using System.Collections;
using System.Collections.ObjectModel;

namespace AppointmentManagementSystem.Data.Repositories;

public class InMemoryCustomerRepository(List<Customer>? initialData = null) : ICustomerRepository
{
    private readonly List<Customer> _customers = initialData ?? [];

    public Task CreateCustomer(Customer customer)
    {
        _customers.Add(customer);
        return Task.CompletedTask;
    }

    public Task DeleteCustomer(int id)
    {
        var foundCustomer = _customers.Find(x => x.Id == id);

        if (foundCustomer is not null)
        {
            _customers.Remove(foundCustomer);
        }

        return Task.CompletedTask;
    }

    public Task<ReadOnlyCollection<Customer>> GetCustomers() => Task.FromResult(_customers.AsReadOnly());

    public Task UpdateCustomer(Customer customer)
    {
        var foundCustomer = _customers.Find(x => x.Id == customer.Id);

        if (foundCustomer is not null)
        {
            foundCustomer.Email = customer.Email;
            foundCustomer.Phone = customer.Phone;
        }

        return Task.CompletedTask;
    }

    public Task<Customer> CustomerExists(int id)
    {
        var foundCustomer = _customers.Find(x => x.Id == id);

        if (foundCustomer is not null)
            return Task.FromResult(foundCustomer);
        else
            return Task.FromResult<Customer>(null);
    }
}
