using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using AppointmentManagementSystem.Data.Abstractions;
using AppointmentManagementSystem.Data.Models;
using AppointmentManagementSystem.Data.Repositories;
using AppointmentManagementSystem.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

class Program
{
    static async Task Main(string[] args)
    {
        string[] actionSelector = ["Create", "Read", "Update", "Delete", "Return"];

        string[] operationSelector = ["Customer Data", "Appointment Data", "Exit"];

        string[] fieldUpdateSelector = ["Email", "Phone"];

        string[] AppointmentUpdateSelector = ["Customer", "Service Type", "Appointment Date", "Appointment Notes"];


        var services = new ServiceCollection();

        List<Customer> initialCustomers = [];
        var cs1 = new Customer(name: "Manos Poulakakis", email: "manolispoulakakis@gmail.com", phone: "6984153487");
        var cs2 = new Customer("Fenia Giannakopoulou", "feniagiannakopoulou@gmail.com", "6943254612");
        var cs3 = new Customer("Kostas Poulakakis", "kospoul@gmail.com", "6982806297");
        initialCustomers.Add(cs1);
        initialCustomers.Add(cs2);
        initialCustomers.Add(cs3);

        var customerRepository = new InMemoryCustomerRepository(initialCustomers);

        List<Appointment> initialAppointment = [];
        // Employess Should not Be assigned on all appointments, only massage has a specific employee
        // Wrong
        Appointment ap1 = new Appointment(cs1, "Massage", new DateTime(2024, 11, 15, 20, 0, 0), "Deep tissue massage");
        Appointment ap2 = new Appointment(cs2, "Massage", new DateTime(2024, 11, 18, 12, 0, 0));
        Appointment ap3 = new Appointment(cs3, "Personal Training", new DateTime(2024, 11, 27, 15, 0, 0), "Full Body Training");

        initialAppointment.Add(ap1);
        initialAppointment.Add(ap2);
        initialAppointment.Add(ap3);
        
        services.AddSingleton<IAppointmentRepository>(x => new InMemoryAppointmentRepository(initialAppointment));
        var serviceProvider = services.BuildServiceProvider();
        var appointmentRepository = serviceProvider.GetService<IAppointmentRepository>();

        string action = string.Empty;
        string operation;
        int customerId;
        int appointmentId;
        // // TimeSpan[] TrainingDuration = new TimeSpan[] {new TimeSpan(0,30,0),new TimeSpan(1,0,0),new TimeSpan(1,30,0)};
        // TimeSpan[] TrainingDuration = new TimeSpan[] {TimeSpan.FromMinutes(30),TimeSpan.FromHours(1),TimeSpan.FromHours(1.5)};
        // TimeSpan trainingDuration = Utilities.DurationSelector(TrainingDuration, "Please Select Training Duration");
        // //TimeSpan trainingDuration = Utilities.CliTimePrompt("Provide Training Duration");
        // AnsiConsole.Markup($"Training Duration = {trainingDuration}\n");
        // MassageType massageType = MassageType.RelaxingMassage;
        // MassageType massageType1 = MassageType.HotStoneMassage;
        // MassageType massageType2= MassageType.Reflexology;
        // if (massageType == MassageType.RelaxingMassage)
        // {
        // }
        // foreach (MassageType type in Enum.GetValues(typeof(MassageType)) )
        // {
        //     if (type == massageType )
        //         AnsiConsole.Markup($"Looping Enumeration,  Type us {type} and massage type is {massageType}\n");
        //     else
        //         AnsiConsole.Markup($"Looping Enumeration,  Type us {type} and massage type is not {massageType}\n");
        // }
        do
        {

            operation = Utilities.Selector(operationSelector, "Please Select Action to be executed or Exit to close the application");
            switch (operation)
            {
                case "Customer Data":
                    do
                    {
                        action = Utilities.Selector(actionSelector, "Please Select Action to be executed or Return to go back");
                        switch (action)
                        {
                            case "Create":
                                Customer customer = new(Utilities.CustomerFields("Full Name"), Utilities.CustomerFields("Email"), Utilities.CustomerFields("Phone"));
                                await customerRepository.CreateCustomer(customer);
                                break;

                            case "Read":
                                Utilities.ReadCustomerData(customerRepository.GetCustomers().Result);
                                break;

                            case "Update":
                                customerId = Utilities.CliIntPrompt("Provide Customer Id");
                                string updateField = Utilities.Selector(fieldUpdateSelector, "Provide Customer Field That Needs To Be Upated");
                                string updateValue = Utilities.CliTextPrompt($"Provide Updated {updateField}");
                                await customerRepository.UpdateCustomer(customerId, updateField, updateValue);
                                break;

                            case "Delete":
                                customerId = Utilities.CliIntPrompt("Provide Customer Id");
                                await customerRepository.DeleteCustomer(customerId);
                                break;

                            default:
                                Utilities.CliWriteToUser("Returning To Start Menu", isErrorMessage: true);
                                break;
                        }
                    } while (action != "Return");
                    break;

                case "Appointment Data":
                    do
                    {
                        action = Utilities.Selector(actionSelector, "Please Select Action to be executed or Return to go back");
                        switch (action)
                        {
                            case "Create":
                                customerId = Utilities.CliIntPrompt("Provide Customer Id");
                                var customer = await customerRepository.CustomerExists(customerId);
                                if (customer is not null)
                                {
                                    var serviceType = Utilities.Selector(["Personal Training", "Massage"], "[bold green]Provide Service Type:[/]");
                                    var appointmentDate = Utilities.CliDatePrompt("Provide Date-Time [red][[MM-DD-YYYY HH:MM]][/]"); 
                                    var appointmentNotes =Utilities.CliTextPrompt("Provide Appointment Notes", isOptional:true);
                                    Appointment appointment = new(customer,serviceType, appointmentDate, appointmentNotes);
                                    await appointmentRepository.CreateAppointment(appointment);
                                }
                                else
                                    Utilities.CliWriteToUser("Customer not Found", isErrorMessage: true);
                                break;

                            case "Read":
                                Utilities.ReadAppointmentsData(appointmentRepository.GetAppointments().Result);
                                break;

                            case "Update":
                                // TODO: Move the following to Utilities, and add validation logic
                                string? updateValue = null ;              // string updateValue;
                                DateTime? updateDate = null ;             // DateTime updateDate;
                                Customer? appointmentCustomer = null;     // Customer appointmentCustomer;
                                appointmentId = Utilities.CliIntPrompt("Provide Appointment Id");
                                bool appointmentExists = await appointmentRepository.AppointmentExists(appointmentId);
                                if (appointmentExists)
                                {
                                    string updateField = Utilities.Selector(AppointmentUpdateSelector, "Provide Appointment Field That Needs To Be Upated");
                                    switch (updateField) 
                                    {
                                        case "Appointment Date":
                                            updateDate = Utilities.CliDatePrompt("Provide Updated Date [red][[MM-DD-YYYY HH:MM]][/]");
                                            // await appointmentRepository.UpdateAppointment(customerId, updateField,updateDateValue: updateDate);
                                            break;

                                        case "Customer":
                                            customerId = Utilities.CliIntPrompt("Provide Updated Customer Id");
                                            appointmentCustomer = await customerRepository.CustomerExists(customerId);
                                            // await appointmentRepository.UpdateAppointment(customerId, updateField,customer: appointmentCustomer);
                                            break;

                                        case "Service Type":
                                            updateValue = Utilities.Selector(["Personal Training", "Massage"], "[bold green]Provide Service Type:[/]");
                                            break;

                                        case "Appointment Notes":
                                            updateValue = Utilities.CliTextPrompt($"Provide Updated {updateField}");
                                            // await appointmentRepository.UpdateAppointment(customerId, updateField,updateValue: updateValue);
                                            break;
                                    }
                                    bool isUpdated = await appointmentRepository.UpdateAppointment(appointmentId, updateField, updateValue, updateDate,appointmentCustomer);
                                    if (isUpdated)
                                        Utilities.CliWriteToUser("Appointment Updated", isErrorMessage: false);
                                    else
                                        Utilities.CliWriteToUser("Appointment couldn't be Updated", isErrorMessage: true);
                                }
                                else
                                    Utilities.CliWriteToUser("Appointment not Found",isErrorMessage: true);
                                break;

                            case "Delete":
                                appointmentId = Utilities.CliIntPrompt("Provide Appointment Id");
                                bool isDeleted = await appointmentRepository.DeleteAppointment(appointmentId);
                                if (isDeleted)
                                   Utilities.CliWriteToUser("Appointment Deleted", isErrorMessage: false);
                                else
                                   Utilities.CliWriteToUser("Appointment not Found",isErrorMessage: true);
                                break;

                            default:
                                Utilities.CliWriteToUser("Returning To Start Menu", isErrorMessage: true);
                                break;

                        }
                    } while (action != "Return");

                    break;
                default:
                    Utilities.CliWriteToUser("Exiting Application", isErrorMessage: true);
                    break;
            }
        } while (operation != "Exit");
    }
}
