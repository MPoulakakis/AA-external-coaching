using System.Reflection;
using AppointmentManagementSystem.Data.Models;
using AppointmentManagementSystem.Data.Repositories;

namespace AppointmentManagementSystem.Repository.Tests;

public class AppointmentRepositoryTests
{

public AppointmentRepositoryTests()
    {
        // Reset _incrementalId before each test
        var fieldInfo = typeof(Appointment).GetField("_incrementalId", BindingFlags.Static | BindingFlags.NonPublic);
        if (fieldInfo != null)
        {
            fieldInfo.SetValue(null, 0);
        }
    }

    [Fact]
    public async void CreateAppointment_And_AddAtAppointmentRepository()
    {
        var AppointmentRepository = new InMemoryAppointmentRepository();
        // AppointmentRepository.CreateAppointment(customer) 
    }
}