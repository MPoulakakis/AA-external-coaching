using System.Globalization;
using AppointmentManagementSystem.Data.Abstractions;
using AppointmentManagementSystem.Data.Models;
using AppointmentManagementSystem.Data.Repositories;
using AppointmentManagementSystem.Utilities;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        
        string[] userAction = ["Create", "Read", "Update", "Delete", "Return"];

        string[] operationSelector = ["Customer Data", "Appointment Data", "Exit"];

        string[] fieldUpdateSelector = ["Email", "Phone"];

        string[] AppointmentUpdateSelector = ["Customer", "Service Type", "Appointment Date", "Appointment Notes"];


        var services = new ServiceCollection();
        // Creating Data For the in Memory Repository
        
        var cs1 = new Customer(name: "Manos Poulakakis", email: "manolispoulakakis@gmail.com", phone: "6984153487");
        var cs2 = new Customer("Fenia Giannakopoulou", "feniagiannakopoulou@gmail.com", "6943254612");
        var cs3 = new Customer("Kostas Poulakakis", "kospoul@gmail.com", "6982806297");
        List<Customer> initialCustomers = [cs1,cs2,cs3];

        var customerRepository = new InMemoryCustomerRepository(initialCustomers);

        Appointment ap1 = new Appointment(cs1, "Massage", new DateTime(2024, 11, 15, 20, 0, 0), "Deep tissue massage");
        Appointment ap2 = new Appointment(cs2, "Massage", new DateTime(2024, 11, 18, 12, 0, 0));
        Appointment ap3 = new Appointment(cs3, "Personal Training", new DateTime(2024, 11, 27, 15, 0, 0), "Full Body Training");
        List<Appointment> initialAppointment = [ap1,ap2,ap3];
        
        services.AddSingleton<IAppointmentRepository>(x => new InMemoryAppointmentRepository(initialAppointment));
        var serviceProvider = services.BuildServiceProvider();
        var appointmentRepository = serviceProvider.GetService<IAppointmentRepository>();

        string actionSelection = string.Empty;
        string operation;
        int customerId;
        int appointmentId;

        do
        {

            operation = ConsoleApp.Selector(operationSelector, "Please Select Action to be executed or Exit to close the application");
            switch (operation)
            {
                case "Customer Data":
                    do
                    {
                        actionSelection = ConsoleApp.Selector(userAction, "Please Select Action to be executed or Return to go back");
                        Enum.TryParse(actionSelection,true,out ActionSelector action);
                        // TODO: Split Everything to methods , to avoid nested statements
                        Customer customer;
                        switch (action)
                        {
                            case ActionSelector.Create:
                                customer = new(ConsoleApp.CustomerFields("Full Name"), ConsoleApp.CustomerFields("Email"), ConsoleApp.CustomerFields("Phone"));
                                await customerRepository.CreateCustomer(customer);
                                break;

                            case ActionSelector.Read:
                                ConsoleApp.ReadCustomerData(customerRepository.GetCustomers().Result);
                                break;

                            case ActionSelector.Update:
                                customerId = ConsoleApp.CliIntPrompt("Provide Customer Id");
                                customer = await customerRepository.CustomerExists(customerId);
                                if (customer is not null)
                                {
                                    customer = ConsoleApp.UpdateCustomerFields(customer);
                                    await customerRepository.UpdateCustomer(customer);
                                }
                                else
                                    ConsoleApp.CliWriteToUser("Customer Not Found","red");
                                break;

                            case ActionSelector.Delete:
                                customerId = ConsoleApp.CliIntPrompt("Provide Customer Id");
                                await customerRepository.DeleteCustomer(customerId);
                                break;

                            default:
                                ConsoleApp.CliWriteToUser("Returning To Start Menu", "red");
                                break;
                        }
                    } while (actionSelection != "Return");
                    break;

                case "Appointment Data":
                    do
                    {
                        Appointment appointment;
                        actionSelection = ConsoleApp.Selector(userAction, "Please Select Action to be executed or Return to go back");
                        Enum.TryParse(actionSelection,true,out ActionSelector action);
                        switch (action)
                        {
                            case ActionSelector.Create:
                                customerId = ConsoleApp.CliIntPrompt("Provide Customer Id");
                                var customer = await customerRepository.CustomerExists(customerId);
                                if (customer is not null)
                                {
                                    var serviceType = ConsoleApp.Selector(["Personal Training", "Massage"], "[bold green]Provide Service Type:[/]");
                                    var appointmentDate = ConsoleApp.CliDatePrompt("Provide Date-Time [red][[MM/DD/YYYY HH:MM]][/]"); 
                                    var appointmentNotes =ConsoleApp.CliTextPrompt("Provide Appointment Notes", isOptional:true);
                                    appointment = new(customer,serviceType, appointmentDate, appointmentNotes);
                                    await appointmentRepository.CreateAppointment(appointment);
                                }
                                else
                                    ConsoleApp.CliWriteToUser("Customer not Found","red");
                                break;

                            case ActionSelector.Read:
                                ConsoleApp.ReadAppointmentsData(appointmentRepository.GetAppointments().Result);
                                break;

                            case ActionSelector.Update:
                                appointmentId = ConsoleApp.CliIntPrompt("Provide Appointment Id");
                                appointment = await appointmentRepository.AppointmentExists(appointmentId);

                                if (appointment is not null)
                                {
                                    ConsoleApp.UpdateAppointmentFields(appointment);
                                    await appointmentRepository.UpdateAppointment(appointment);
                                }
                                else
                                    ConsoleApp.CliWriteToUser("Appointment not Found","red");
                                break;

                            case ActionSelector.Delete:
                                appointmentId = ConsoleApp.CliIntPrompt("Provide Appointment Id");
                                bool isDeleted = await appointmentRepository.DeleteAppointment(appointmentId);
                                
                                if (isDeleted)
                                   ConsoleApp.CliWriteToUser("Appointment Deleted");
                                else
                                   ConsoleApp.CliWriteToUser("Appointment not Found","red");
                                break;

                            default:
                                ConsoleApp.CliWriteToUser("Returning To Start Menu","red");
                                break;

                        }
                    } while (actionSelection != "Return");

                    break;
                default:
                    ConsoleApp.CliWriteToUser("Exiting Application", "red");
                    break;
            }
        } while (operation != "Exit");
    }
}
