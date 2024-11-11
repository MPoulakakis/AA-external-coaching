using System.Reflection;
using AppointmentManagementSystem.Data.Models;
using AppointmentManagementSystem.Data.Repositories;

namespace AppointmentManagementSystem.Repository.Tests;

public class CustomerRepositoryTests
{

public CustomerRepositoryTests()
    {
        // Reset _incrementalId before each test
        var fieldInfo = typeof(Customer).GetField("_incrementalId", BindingFlags.Static | BindingFlags.NonPublic);
        if (fieldInfo != null)
        {
            fieldInfo.SetValue(null, 0);
        }
    }
    
    [Fact]
    public async void ReadCustomerFromRepositoryData()
    {
        // Arrange
        Customer customer1 = new(name: "Manos Poulakakis",email:"manolispoulakakis@gmaiil.com",phone:"6984153487");
        //Customer customer2 = new(name: "Giwrgos Tsaparas",email:"gtsap@gmaiil.com",phone:"6972894572");
        List<Customer> customers = [customer1];
        var customerRepository = new InMemoryCustomerRepository(customers);
        // Act
        var customersRepositoryData = customerRepository.GetCustomers().Result;        
        // Assert
        foreach (Customer client in customersRepositoryData)
        {
            Assert.Equal(1,client.Id);
            Assert.Equal("Manos Poulakakis",client.Name);
            Assert.Equal("manolispoulakakis@gmaiil.com",client.Email);
            Assert.Equal("6984153487",client.Phone);
        }
    }

    [Fact]
    public async Task CreateCustomer_And_AddToRepository()
    {
        // Arrange
        Customer customer1 = new(name: "Manos Poulakakis",email:"manolispoulakakis@gmaiil.com",phone:"6984153487");
        var customerRepository = new InMemoryCustomerRepository();        
        // Act
        await customerRepository.CreateCustomer(customer1);
        var customersRepositoryData = customerRepository.GetCustomers().Result;        
        // Assert
        foreach (Customer client in customersRepositoryData)
        {
            Assert.Equal(1,client.Id); // static field increases id
            Assert.Equal("Manos Poulakakis",client.Name);
            Assert.Equal("manolispoulakakis@gmaiil.com",client.Email);
            Assert.Equal("6984153487",client.Phone);
        }

    }

    [Fact]
    public async Task CheckIfCustomerExists_OrNot()
    {
        // Arrange
        Customer customer1 = new(name: "Manos Poulakakis",email:"manolispoulakakis@gmaiil.com",phone:"6984153487");
        Customer customer2 = new(name: "Gtsap",email:"gtsap@gmaiil.com",phone:"2106523398");
        var customerRepository = new InMemoryCustomerRepository([customer1,customer2]);
        //Act
        Customer customer = await customerRepository.CustomerExists(1);
        Customer customer3 = await customerRepository.CustomerExists(3);
        //Assert
        Assert.NotNull(customer);
        Assert.Equal(customer, customer1);
        Assert.NotEqual(customer, customer2);
        Assert.Null(customer3);
    }

    [Fact]
    public async Task DeleteCustomerFromRepository()
    {
        Customer customer1 = new(name: "Manos Poulakakis",email:"manolispoulakakis@gmaiil.com",phone:"6984153487");
        var customerRepository = new InMemoryCustomerRepository([customer1]);
        await customerRepository.DeleteCustomer(1);
        var customersRepositoryData = await customerRepository.GetCustomers();
        bool customerExists = false;

        foreach (var customer in customersRepositoryData)
        {
            if (customer.Id == 1)
                customerExists = true;
        }

        Assert.False(customerExists);
    }

    [Fact]
    public async Task UpdateCustomerInfoFromRepository()
    {
        Customer customer1 = new(name: "Manos Poulakakis",email:"manolispoulakakis@gmaiil.com",phone:"6984153487");
        var customerRepository = new InMemoryCustomerRepository([customer1]);
        await customerRepository.UpdateCustomer(1,"Email","nkorompos@gmail.com");
        await customerRepository.UpdateCustomer(1,"Phone","6945348712");
        var customersRepositoryData = customerRepository.GetCustomers().Result;
        
        string email = customer1.Email;
        string name = customer1.Name;
        string phone = customer1.Phone;
        
        foreach (var customer in customersRepositoryData)
        {
            email = customer.Email;
            phone = customer.Phone;
        }

        Assert.Equal("nkorompos@gmail.com",email);
        Assert.Equal("6945348712",phone);
    }

}