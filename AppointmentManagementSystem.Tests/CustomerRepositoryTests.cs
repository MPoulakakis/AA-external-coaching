using System.Reflection;
using AppointmentManagementSystem.Data.Models;
using AppointmentManagementSystem.Data.Repositories;

namespace AppointmentManagementSystem.Tests;

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
    public async void ReadCustomerRepositoryData()
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
    public async Task AddCustomerToRepository()
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
    public async Task CheckIfCustomerExists()
    {
        // Arrange
        Customer customer1 = new(name: "Manos Poulakakis",email:"manolispoulakakis@gmaiil.com",phone:"6984153487");
        Customer customer2 = new(name: "Gtsap",email:"gtsap@gmaiil.com",phone:"2106523398");
        
        List<Customer> customers = [customer1];
        var customerRepository = new InMemoryCustomerRepository(customers);
        //Act
        Customer customer = await customerRepository.CustomerExists(1);
        //Assert
        Assert.NotNull(customer);
        Assert.Equal(customer, customer1);
        Assert.NotEqual(customer, customer2);

    }
    
    [Fact]
        public async Task CheckIfCustomerDoesnotExists()
    {
        // Arrange
        Customer customer1 = new(name: "Manos Poulakakis",email:"manolispoulakakis@gmaiil.com",phone:"6984153487");
        Customer customer2 = new(name: "Gtsap",email:"gtsap@gmaiil.com",phone:"2106523398");
        
        List<Customer> customers = [customer1];
        var customerRepository = new InMemoryCustomerRepository(customers);
        //Act
        Customer customer = await customerRepository.CustomerExists(3);
        //Assert
        Assert.Null(customer);
    }
}