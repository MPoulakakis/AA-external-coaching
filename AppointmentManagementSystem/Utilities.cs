using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Text.RegularExpressions;
using AppointmentManagementSystem.Data.Models;
using Spectre.Console;
namespace AppointmentManagementSystem.Utilities
{
    class Utilities
    {
        public static string Selector(string[] Selector, string Title) 
        {
            string actionSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[bold green][/]\n")
            .PageSize(5)
            .AddChoices(Selector));
        return actionSelection;
        }


        public static string CliTextPrompt(string promptText)
        {
            string promptValue = AnsiConsole.Prompt( new TextPrompt<string>($"[bold green]{promptText}:[/] "));
            return promptValue;
        }


        public static DateTime CliDatePrompt(string promptText)
        {
            DateTime promptValue = AnsiConsole.Prompt( new TextPrompt<DateTime>($"[bold green]{promptText}:[/] "));
            return promptValue;
        }
        
        public static int CliIntPrompt(string promptText)
        {
            int promptValue = AnsiConsole.Prompt( new TextPrompt<int>($"[bold green]{promptText}:[/] "));
            return promptValue;
        }


        public static Table CreateDataTable(string [] TableFields)
        {
            var customerTable = new Table();
            foreach (string field in TableFields)
            {
                customerTable.AddColumn(new TableColumn($"[bold green]{field}[/]").Centered());
            }
            return customerTable;
        }


        public static string CustomerFields(string customerField)
        {
            string field;
            switch (customerField) 
            {
                case "Email":
                    var emailPattern = @"^[^@\s]+@(gmail|hotmail|yahoo)\.(com|gr)$";
                    field = AnsiConsole.Prompt(
                        new TextPrompt<string>($"[bold green]Provide Customer {customerField}:[/] ")
                        .Validate(input =>
                        {
                            if (!Regex.IsMatch(input,emailPattern))
                            {
                                return ValidationResult.Error("[red]Email should contain @,.com,.gr, and a mailing organization[/]");
                            }
                            return ValidationResult.Success();
                        }
                        ));
                    return field;
                case "Phone":
                    field = AnsiConsole.Prompt(
                        new TextPrompt<string>($"[bold green]Provide Customer {customerField}:[/] ")
                        .Validate(input => 
                        {
                            if ((input.Length != 10) || (!input.All(char.IsDigit)))
                                return ValidationResult.Error("[red]Phone Number should contain 10 Digits[/]");
                            
                            if (! input.StartsWith("69"))
                                return ValidationResult.Error("[red]Phone Number should start with 69[/]");
                            
                            return ValidationResult.Success();
                        }
                        ));
                    return field;
                default:
                    field = AnsiConsole.Prompt(
                        new TextPrompt<string>($"[bold green]Provide Customer {customerField}:[/] "));
                    return field;
            }
        }


        public static void ReadAppointmentsData(ReadOnlyCollection<Appointment> appointmentsList)
        {
            //Table appointmentTable = AppointmentCustomerDataTable();
            Table appointmentTable = CreateDataTable(["Id","Full Name","Service Type","Appointment Date","Appointment Notes"]);
            foreach (Appointment appointment in appointmentsList.AsReadOnly())
            {
                appointmentTable.AddRow(
                    new Markup($"[magenta]{appointment.Id}[/]"),
                    new Markup($"[magenta]{appointment.Customer.Name}[/]"),
                    new Markup($"[magenta]{appointment.ServiceType}[/]"),
                    new Markup($"[magenta]{appointment.AppointmentDate}[/]"),
                    new Markup($"[magenta]{appointment.AppointmentNotes}[/]")
                    );

            }
            AnsiConsole.Write(appointmentTable);
        }


        public static void ReadCustomerData(ReadOnlyCollection<Customer> customersList)
        {
            Table customerTable = CreateDataTable(["Id","Full Name","Email","Phone"]);
            foreach (Customer customers in customersList.AsReadOnly())
            {
                customerTable.AddRow(
                    new Markup($"[magenta]{customers.Id}[/]"),
                    new Markup($"[magenta]{customers.Name}[/]"),
                    new Markup($"[magenta]{customers.Email}[/]"),
                    new Markup($"[magenta]{customers.Phone}[/]")
                    );

            }
            AnsiConsole.Write(customerTable);
        }
    }

}