using AppointmentManagementSystem.Data;
using AppointmentManagementSystem.Data.Abstractions;
using AppointmentManagementSystem.Data.Enums;
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
        string[] appointmentTypeChoices = ["Personal Training","Massage"];
        string[] trainingDurationChoices = ["Thirty Minutes","Sixty Minutes", "Ninety Minutes"];

        var services = new ServiceCollection();
        // Creating Data For the in Memory Repository
        services.AddSingleton(typeof(IAppointmentRepository<>),typeof(InMemoryAppointmentRepository<>));
        services.AddSingleton<ICustomerRepository,InMemoryCustomerRepository>();
        var serviceProvider = services.BuildServiceProvider();
        var customerRepository = serviceProvider.GetRequiredService<ICustomerRepository>();
        // var personalTrainingAppointmentRepository = serviceProvider.GetRequiredService<IAppointmentRepository<PersonalTrainingAppointment>>();
        // var massageAppointmentRepository = serviceProvider.GetRequiredService<IAppointmentRepository<MassageAppointment>>();
        var AppointmentRepository = serviceProvider.GetRequiredService<IAppointmentRepository<Appointment>>();
        //var AppointmentRepository =  InitialData.GetInitialAppointments<MassageAppointment>();

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
                        actionSelection = ConsoleApp.Selector(userAction, "Please Select Action to be executed or Return to go back");
                        Enum.TryParse(actionSelection,true,out ActionSelector action);
                        switch (action)
                        {
                            case ActionSelector.Create:
                                customerId = ConsoleApp.CliIntPrompt("Provide Customer Id");
                                var customer = await customerRepository.CustomerExists(customerId);
                                if (customer is not null)
                                {
                                    var appointmentDate = ConsoleApp.CliDatePrompt("Provide Date-Time [red][[MM/DD/YYYY HH:MM]][/]"); 
                                    var appointmentNotes = ConsoleApp.CliTextPrompt("Provide Appointment Notes", isOptional:true);
                                    AppointmentType appointmentType = ConsoleApp.EnumSelector<AppointmentType>("Select Appointment Type",appointmentTypeChoices);
                                    switch (appointmentType) 
                                    {
                                        case AppointmentType.PersonalTraining:
                                            var duration = ConsoleApp.EnumSelector<trainingDuration>("Select Training Duration",trainingDurationChoices);
                                            string focusedTraining  = ConsoleApp.CliTextPrompt("Focused Muscle Group(s)", isOptional:true);
                                            string injuriesComments = ConsoleApp.CliTextPrompt("Injury Report", isOptional:true);
                                            PersonalTrainingAppointment personalTrainingAppointment = new(customer,duration, appointmentDate,focusedTraining,injuriesComments, appointmentNotes);
                                            await AppointmentRepository.CreateAppointment(personalTrainingAppointment);
                                            
                                            break;
                                    
                                        case AppointmentType.Massage:
                                            MassageType massageType = ConsoleApp.EnumSelector<MassageType>("Select Massage Type",["Relaxing","Reflexology","Hot Stone"]);
                                            EmployeeGender employeeGender = ConsoleApp.EnumSelector<EmployeeGender>("Select Employee Sex",["Male","Female"]);
                                            MassageAppointment massageAppointment = new(customer, massageType, employeeGender, appointmentDate, appointmentNotes);
                                            await AppointmentRepository.CreateAppointment(massageAppointment);
                                            break;
                                    }
                                }
                                else
                                    ConsoleApp.CliWriteToUser("Customer not Found","red");
                                break;

                            case ActionSelector.Read:
                                ConsoleApp.ReadAppointmentsData(await AppointmentRepository.GetAppointments());
                                //ConsoleApp.ReadAppointmentsData(await AppointmentRepository.GetAppointments());
                                break;

                            case ActionSelector.Update:
                                AppointmentType appointmentTypeToUpdate = ConsoleApp.EnumSelector<AppointmentType>("Select Appointment Type",appointmentTypeChoices);
                                appointmentId = ConsoleApp.CliIntPrompt("Provide Appointment Id");
                                switch (appointmentTypeToUpdate) 
                                {
                                    case AppointmentType.PersonalTraining:
                                        var personalAppointment = await AppointmentRepository.AppointmentExists(appointmentId);
                                        if (personalAppointment is not null)
                                        {
                                            ConsoleApp.UpdateAppointmentFields(personalAppointment);
                                            await AppointmentRepository.UpdateAppointment(personalAppointment);
                                        }
                                        else
                                            ConsoleApp.CliWriteToUser("Appointment not Found","red");
                                        break;
                                
                                    case AppointmentType.Massage:
                                        var massageAppointment = await AppointmentRepository.AppointmentExists(appointmentId);
                                        if (massageAppointment is not null)
                                        {
                                            ConsoleApp.UpdateAppointmentFields(massageAppointment);
                                            await AppointmentRepository.UpdateAppointment(massageAppointment);
                                        }
                                        else
                                            ConsoleApp.CliWriteToUser("Appointment not Found","red");
                                        break;
                                }

                                break;

                            case ActionSelector.Delete:
                                AppointmentType appointmentTypeToDelete = ConsoleApp.EnumSelector<AppointmentType>("Select Appointment Type",appointmentTypeChoices);
                                appointmentId = ConsoleApp.CliIntPrompt("Provide Appointment Id");
                                bool isDeleted = false;
                                switch (appointmentTypeToDelete) 
                                {
                                    case AppointmentType.PersonalTraining:
                                        isDeleted = await AppointmentRepository.DeleteAppointment(appointmentId);
                                        break;
                                
                                    case AppointmentType.Massage:
                                        isDeleted = await AppointmentRepository.DeleteAppointment(appointmentId);
                                        break;
                                }
                                if (isDeleted)
                                   ConsoleApp.CliWriteToUser("Appointment Deleted", "bold yellow");
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
