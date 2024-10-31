﻿using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using AppointmentManagementSystem.Data.Models;
using AppointmentManagementSystem.Data.Repositories;
using AppointmentManagementSystem.Utilities;
using Spectre.Console;

class Program
{
    static async Task Main(string[] args)
    {
        // Render a table using Spectre.Console
        // var table = new Table();
        // table.AddColumn("Foo");
        // table.AddColumn("Bar");

        // table.AddRow("Baz", "[green]Qux[/]");

        // AnsiConsole.Write(table);
        string[] actionSelector = ["Create", "Read", "Update", "Delete", "Return"];

        string[] operationSelector = ["Customer Data", "Appointment Data", "Exit"];

        string[] fieldUpdateSelector = ["Full Name", "Email", "Phone"];

        string[] AppointmentUpdateSelector = ["Customer", "Service Type", "Appointment Date", "Appointment Notes"];


        List<Customer> initialCustomers = [];
        var ex1 = new Customer(name: "Manos Poulakakis", email: "manolispoulakakis@gmail.com", phone: "6984153487");
        var ex2 = new Customer("Fenia Giannakopoulou", "feniagiannakopoulou@gmail.com", "6943254612");
        var ex3 = new Customer("Kostas Poulakakis", "kospoul@gmail.com", "6982806297");
        initialCustomers.Add(ex1);
        initialCustomers.Add(ex2);
        initialCustomers.Add(ex3);

        var customerRepository = new InMemoryCustomerRepository(initialCustomers);


        List<Appointment> initialAppointment = [];
        Appointment ap1 = new Appointment(ex1, "Massage", new DateTime(2024, 11, 15, 20, 0, 0), "Deep tissue massage");
        Appointment ap2 = new Appointment(ex2, "Massage", new DateTime(2024, 11, 18, 12, 0, 0));
        Appointment ap3 = new Appointment(ex3, "Personal Training", new DateTime(2024, 11, 27, 15, 0, 0), "Full Body Training");

        initialAppointment.Add(ap1);
        initialAppointment.Add(ap2);
        initialAppointment.Add(ap3);

        var appointmentRepository = new InMemoryAppointmentRepository(initialAppointment);


        string action = string.Empty;
        string operation;
        int customerId;
        int appointmentId;
        do
        {

            operation = Utilities.Selector(operationSelector, "Please Select Action to be executed or Exit to close the application");
            switch (operation)
            {
                case "Customer Data":
                    AnsiConsole.Markup($"[bold green]You Selected [red]{operation}[/][/]\n");
                    do
                    {
                        action = Utilities.Selector(actionSelector, "Please Select Action to be executed or Exit to close the application");
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
                                AnsiConsole.Markup($"[bold red]Returning To Start Menu[/]\n");
                                break;
                        }
                    } while (action != "Return");
                    break;

                case "Appointment Data":
                    AnsiConsole.Markup($"[bold green]You Selected [red]{operation}[/][/]\n");
                    do
                    {
                        action = Utilities.Selector(actionSelector, "Please Select Action to be executed or Exit to close the application");
                        switch (action)
                        {
                            case "Create":
                                customerId = Utilities.CliIntPrompt("Provide Customer Id");
                                var customer = await customerRepository.CustomerExists(customerId);
                                if (customer is not null)
                                {
                                    var serviceType = Utilities.Selector(["Personal Training", "Massage"], "[bold green]Provide Service Type:[/]");
                                    
                                    var appointmentDate = AnsiConsole.Prompt(
                                        new TextPrompt<DateTime>($"[bold green]Provide Date-Time [/][red][[MM-DD-YYYY HH:MM]]:[/] "));

                                    var appointmentNotes = AnsiConsole.Prompt(
                                        new TextPrompt<string>($"[red][[Optional]][/][bold green]Provide Appointment Notes:[/] ")
                                        .AllowEmpty());
                                    
                                    Appointment appointment = new(customer,serviceType, appointmentDate, appointmentNotes);
                                    await appointmentRepository.CreateAppointment(appointment, customerId);
                                }
                                else
                                {
                                    AnsiConsole.Markup($"[bold red]Customer not Found[/]\n");
                                }
                                break;
                            case "Read":
                                Utilities.ReadAppointmentsData(appointmentRepository.GetAppointments().Result);
                                break;
                            case "Update":
                                string? updateValue = null ;              // string updateValue;
                                DateTime? updateDate = null ;             // DateTime updateDate;
                                Customer? appointmentCustomer = null;     // Customer appointmentCustomer;
                                appointmentId = Utilities.CliIntPrompt("Provide Appointment Id");
                                string updateField = Utilities.Selector(AppointmentUpdateSelector, "Provide Appointment Field That Needs To Be Upated");
                                switch (updateField) {
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
                                    default:
                                        updateValue = Utilities.CliTextPrompt($"Provide Updated {updateField}");
                                        // await appointmentRepository.UpdateAppointment(customerId, updateField,updateValue: updateValue);
                                        break;
                                }
                                await appointmentRepository.UpdateAppointment(appointmentId, updateField, updateValue, updateDate,appointmentCustomer);
                                break;
                            case "Delete":
                                appointmentId = Utilities.CliIntPrompt("Provide Appointment Id");
                                await appointmentRepository.DeleteAppointment(appointmentId);
                                break;
                            default:
                                AnsiConsole.Markup($"[bold red]Returning To Start Menu[/]\n");
                                break;

                        }

                    } while (action != "Return");

                    break;
                default:
                    AnsiConsole.Markup($"[bold red]Exiting Application[/]\n");
                    break;
            }
        } while (operation != "Exit");
    }

    // Add parameters on the method to use it for every selection prompt
    // parameters should be the < title , list of choices> 
    // this can be use for both operations and actions

    // static string createCustomer()
    // {
    //     string name = AnsiConsole.Prompt(
    //         new TextPrompt<string>($"Provide Customer Name"));
    //     string email = AnsiConsole.Prompt(
    //         new TextPrompt<string>($"Provide Customer Email"));
    //     string phone = AnsiConsole.Prompt(
    //         new TextPrompt<string>($"Provide Customer Phone Number"));

    // }

    // Pass an array to method as parameter to be used for choices ( use array due to standard size )
    // By doing so we can use the actionSelector for all selection prompt cases
    // declare array like , string[] actionSelector
}