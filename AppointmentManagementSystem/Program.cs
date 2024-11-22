using AppointmentManagementSystem;
using AppointmentManagementSystem.Data.Abstractions;
using AppointmentManagementSystem.Data.Enums;
using AppointmentManagementSystem.Data.Models;
using AppointmentManagementSystem.Data.Repositories;
using AppointmentManagementSystem.ServicesHelper;
using Microsoft.Extensions.DependencyInjection;


class Program
{
    static async Task Main(string[] args)
    {
        
        string[] userAction = ["Create", "Read", "Update", "Delete", "Return"];
        string[] operationSelector = ["Customers Data", "Appointments Data","Reports Data", "Exit"];
        string[] appointmentTypeChoices = ["Personal Training","Massage"];
        string[] trainingDurationChoices = ["Thirty Minutes","Sixty Minutes", "Ninety Minutes"];
        var services = new ServiceCollection();
        // Creating Data For the in Memory Repository
        services.AddSingleton(typeof(IAppointmentRepository<>),typeof(InMemoryAppointmentRepository<>));
        services.AddSingleton<ICustomerRepository,InMemoryCustomerRepository>();
        var serviceProvider = services.BuildServiceProvider();
        var customerRepository = serviceProvider.GetRequiredService<ICustomerRepository>();
        var appointmentRepository = serviceProvider.GetRequiredService<IAppointmentRepository<Appointment>>();

        string actionSelection;
        // string operation;
        OperationSelector operation;
        int customerId;
        int appointmentId;
        ReportSelector report;
        CustomerReportsService customerViewModel = new(customerRepository); 
        AppointmentReportService appointmentViewModel = new(appointmentRepository); 
        

        do
        {
            //operation = ConsoleApp.Selector(operationSelector, "Please Select Action to be executed or Exit to close the application");
            operation = ConsoleApp.EnumSelector<OperationSelector>("Please Select Action to be executed or Exit to close the application",operationSelector);
            switch (operation)
            {
                case OperationSelector.CustomersData:
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

                case OperationSelector.AppointmentsData:
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
                                    ServiceType appointmentType = ConsoleApp.EnumSelector<ServiceType>("Select Appointment Type",appointmentTypeChoices);
                                    switch (appointmentType) 
                                    {
                                        case ServiceType.PersonalTraining:
                                            var duration = ConsoleApp.EnumSelector<TrainingDuration>("Select Training Duration",trainingDurationChoices);
                                            string focusedTraining  = ConsoleApp.CliTextPrompt("Focused Muscle Group(s)", isOptional:true);
                                            string injuriesComments = ConsoleApp.CliTextPrompt("Injury Report", isOptional:true);
                                            PersonalTrainingAppointment personalTrainingAppointment = new(customer,duration, appointmentDate,focusedTraining,injuriesComments, appointmentNotes);
                                            await appointmentRepository.CreateAppointment(personalTrainingAppointment);
                                            
                                            break;
                                    
                                        case ServiceType.Massage:
                                            MassageType massageType = ConsoleApp.EnumSelector<MassageType>("Select Massage Type",["Relaxing","Reflexology","Hot Stone"]);
                                            EmployeeGender employeeGender = ConsoleApp.EnumSelector<EmployeeGender>("Select Employee Sex",["Male","Female"]);
                                            MassageAppointment massageAppointment = new(customer, massageType, employeeGender, appointmentDate, appointmentNotes);
                                            await appointmentRepository.CreateAppointment(massageAppointment);
                                            break;
                                    }
                                }
                                else
                                    ConsoleApp.CliWriteToUser("Customer not Found","red");
                                break;

                            case ActionSelector.Read:
                                ConsoleApp.ReadAppointmentsData(await appointmentRepository.GetAppointments());                                
                                break;

                            case ActionSelector.Update:
                                ServiceType appointmentTypeToUpdate = ConsoleApp.EnumSelector<ServiceType>("Select Appointment Type",appointmentTypeChoices);
                                appointmentId = ConsoleApp.CliIntPrompt("Provide Appointment Id");
                                switch (appointmentTypeToUpdate) 
                                {
                                    case ServiceType.PersonalTraining:
                                        var personalAppointment = await appointmentRepository.AppointmentExists(appointmentId);
                                        if (personalAppointment is not null)
                                        {
                                            ConsoleApp.UpdateAppointmentFields(personalAppointment);
                                            await appointmentRepository.UpdateAppointment(personalAppointment);
                                        }
                                        else
                                            ConsoleApp.CliWriteToUser("Appointment not Found","red");
                                        break;
                                
                                    case ServiceType.Massage:
                                        var massageAppointment = await appointmentRepository.AppointmentExists(appointmentId);
                                        if (massageAppointment is not null)
                                        {
                                            ConsoleApp.UpdateAppointmentFields(massageAppointment);
                                            await appointmentRepository.UpdateAppointment(massageAppointment);
                                        }
                                        else
                                            ConsoleApp.CliWriteToUser("Appointment not Found","red");
                                        break;
                                }

                                break;

                            case ActionSelector.Delete:
                                ServiceType appointmentTypeToDelete = ConsoleApp.EnumSelector<ServiceType>("Select Appointment Type",appointmentTypeChoices);
                                appointmentId = ConsoleApp.CliIntPrompt("Provide Appointment Id");
                                bool isDeleted = false;
                                switch (appointmentTypeToDelete) 
                                {
                                    case ServiceType.PersonalTraining:
                                        isDeleted = await appointmentRepository.DeleteAppointment(appointmentId);
                                        break;
                                
                                    case ServiceType.Massage:
                                        isDeleted = await appointmentRepository.DeleteAppointment(appointmentId);
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

                case OperationSelector.ReportsData:

                    do
                    {
                        report = ConsoleApp.EnumSelector<ReportSelector>("Select Report Operation or Return to Go Back");
                        string mainColor = "bold blue";
                        string fillerColor = "bold grey100";
                        switch (report)
                        {
                            case ReportSelector.RegisteredCustomers:
                                ConsoleApp.CliWriteToUser($"We have [{fillerColor}]{await customerRepository.GetCustomersCount()}[/] Registered Customers",mainColor);
                                break;
                        
                            case ReportSelector.CustomersByRegistrationDate:
                                DateTime date = ConsoleApp.CliDatePrompt("Provide Date for Register Report");
                                var registeredCustomers = await customerRepository.GetCustomersByRegistrationDate(date);
                                    ConsoleApp.ReadCustomerData(registeredCustomers.AsReadOnly());
                                break;
                        
                            case ReportSelector.CountAppointments:
                                int appointmentCount = await appointmentRepository.GetAppointmentsCount<Appointment>();
                                ConsoleApp.CliWriteToUser($"We Have [{fillerColor}]{appointmentCount}[/] Total Appointments",mainColor);
                                break;
                        
                            case ReportSelector.CountMassageAppointments:
                                int massageCount = await appointmentRepository.GetAppointmentsCount<MassageAppointment>();
                                ConsoleApp.CliWriteToUser($"We Have [{fillerColor}]{massageCount}[/] Total Massage Appointments",mainColor);
                                break;
                        
                            case ReportSelector.CountPersonalAppointments:
                                int personalCount = await appointmentRepository.GetAppointmentsCount<PersonalTrainingAppointment>();
                                ConsoleApp.CliWriteToUser($"We Have [{fillerColor}]{personalCount}[/] Total Personal Appointments",mainColor);
                                break;
                        
                            case ReportSelector.EmployeeGenderPreference:
                                int preference = await appointmentRepository.GetEmployeeGenderPreference();
                                if (preference == 2)
                                    ConsoleApp.CliWriteToUser($"Users Prefer Both [{fillerColor}]Male[/] and [{fillerColor}]Female[/] Massauer Equally",mainColor);
                                else
                                    ConsoleApp.CliWriteToUser($"Users Prefer [{fillerColor}]{(EmployeeGender)preference}[/] Massauer",mainColor);
                                break;
                        
                            case ReportSelector.TrainingDurationPreference:
                                var durations = await appointmentRepository.GetTrainingDuration();
                                if (durations.Count > 1)
                                {
                                    ConsoleApp.CliWriteToUser("Customers Prefers More Than One Training Duration",mainColor);
                                    foreach(var duration in durations)
                                        ConsoleApp.CliWriteToUser($"{ConsoleApp.AddSpacesToEnums(duration)}",$"{fillerColor}");
                                }
                                else
                                    ConsoleApp.CliWriteToUser($"Most Prefered Training duration is [{fillerColor}]{ConsoleApp.AddSpacesToEnums(durations[0])}[/]");
                                    break;
                        
                        
                            case ReportSelector.MaxMassageAppointmentsDate:
                                var prefMassageDates = await appointmentRepository.GetPreferedMassageDates();
                                if (prefMassageDates.Count > 1 && prefMassageDates.Count != 0)
                                {
                                    ConsoleApp.CliWriteToUser($"Dates with Most Massage Appointments are",mainColor);
                                    foreach (var dates in prefMassageDates)
                                    {
                                        ConsoleApp.CliWriteToUser($"{dates} ({dates.DayOfWeek})",fillerColor);
                                    }
                                }
                                else if ( prefMassageDates.Count == 1)
                                    ConsoleApp.CliWriteToUser($"Date with Most Massage Appointments is : [{fillerColor}]{prefMassageDates[0]} ({prefMassageDates[0].DayOfWeek})[/]");
                                    break;
                        
                        
                            case ReportSelector.MaxPersonalAppointmentsDate:
                                var prefPersonalDates = await appointmentRepository.GetPreferedPersonalDates();
                                if (prefPersonalDates.Count > 1)
                                {
                                    ConsoleApp.CliWriteToUser($"Dates with Most Personal Training Appointments are", mainColor);
                                    foreach (var dates in prefPersonalDates)
                                    {
                                        ConsoleApp.CliWriteToUser($"{dates} ({dates.DayOfWeek})",fillerColor);
                                    }
                                }
                                else if (prefPersonalDates.Count == 1)
                                    ConsoleApp.CliWriteToUser($"Date with Most Personal Training Appointments is : [{fillerColor}]{prefPersonalDates[0]} ({prefPersonalDates[0].DayOfWeek})[/]");
                                break;
                        
                        
                            case ReportSelector.MassageTypePreference:
                                var prefMassageType = await appointmentRepository.GetPreferedMassageType();
                                if (prefMassageType.Count > 1)
                                    ConsoleApp.CliWriteToUser("Prefered Massaged Types are",mainColor);
                                    foreach (var type in prefMassageType )
                                    {
                                        ConsoleApp.CliWriteToUser($"{type}",fillerColor);
                                    }                               
                                break;
                        
                            case ReportSelector.MaxAppointmentsDate:
                                var maxAppointmentsDate = await appointmentRepository.GetDateWithMostAppointments();
                                if(maxAppointmentsDate.Count > 1)
                                {
                                    ConsoleApp.CliWriteToUser($"Dates with Most Appointments are",mainColor);
                                    foreach (var d in maxAppointmentsDate)
                                    {
                                        ConsoleApp.CliWriteToUser($"{d} ({d.DayOfWeek})",fillerColor);
                                    }
                                }
                                else
                                    ConsoleApp.CliWriteToUser($"Date with Most Appointments is {maxAppointmentsDate[0]} ({maxAppointmentsDate[0].DayOfWeek})",mainColor);
                                break;
                        
                            case ReportSelector.MinAppointmentsDate:
                                var minAppointmentsDate = await appointmentRepository.GetDateWithLeastAppointments();
                                if(minAppointmentsDate.Count > 1)
                                {
                                    ConsoleApp.CliWriteToUser($"Dates with Least Appointments are",mainColor);
                                    foreach (var d in minAppointmentsDate)
                                    {
                                        ConsoleApp.CliWriteToUser($"{d} ({d.DayOfWeek})",fillerColor);
                                    }
                                }
                                else
                                    ConsoleApp.CliWriteToUser($"Date with Least Appointments is {minAppointmentsDate[0]} ({minAppointmentsDate[0].DayOfWeek})",mainColor);
                                break;
                        
                            case ReportSelector.Return:
                                break;
                        }
                    } while(report != ReportSelector.Return);

                    break;

                case OperationSelector.Exit:
                    ConsoleApp.CliWriteToUser("Exiting Application", "red");
                    break;
            }
        } while (operation != OperationSelector.Exit);
    }

}
