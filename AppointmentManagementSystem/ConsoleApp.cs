using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Globalization;
using AppointmentManagementSystem.Data.Models;
using Spectre.Console;

namespace AppointmentManagementSystem.Utilities
{
    class ConsoleApp
    {
        public static string Selector(string[] Selector, string Title) 
        {
            string actionSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"[bold green]{Title}[/]\n")
            .PageSize(5)
            .AddChoices(Selector));
        return actionSelection;
        }
        
        public static TimeSpan DurationSelector(TimeSpan[] TrainingDuration, string Title) 
        {
            TimeSpan trainingDuration = AnsiConsole.Prompt(
            new SelectionPrompt<TimeSpan>()
            .Title($"[bold green]{Title}[/]\n")
            .PageSize(5)
            .AddChoices(TrainingDuration));
        return trainingDuration;
        }
    // TODO: For all Cli Prompt Functions unify in class?
        
        public static string CliTextPrompt(string promptText,bool isOptional = false)
        {
            string promptValue;
            if (isOptional)
                promptValue = AnsiConsole.Prompt( new TextPrompt<string>($"[red][[Optional]][/][bold green]{promptText}:[/] ")
                .AllowEmpty()
                );
            else
                promptValue = AnsiConsole.Prompt( new TextPrompt<string>($"[bold green]{promptText}:[/] "));
            return promptValue;
        }
        
        public static DateTime CliDatePrompt(string promptText)
        {
            DateTime promptValue = AnsiConsole.Prompt( new TextPrompt<DateTime>($"[bold green]{promptText}:[/] "));
            return promptValue;
        }
        
        public static TimeSpan CliTimePrompt(string promptText)
        {
            TimeSpan trainingDuration = AnsiConsole.Prompt(new TextPrompt<TimeSpan>("Duration Time")
            .AddChoice(new TimeSpan(0,30,0))
            .AddChoice(new TimeSpan(1,0,0))
            .AddChoice(new TimeSpan(1,30,0))
            );
            return trainingDuration;
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

        public static void CliWriteToUser(string messageUser,string textColor = "green")
        {
            AnsiConsole.Markup($"[{textColor}]{messageUser}[/]\n");
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

        public static Customer UpdateCustomerFields(Customer customer)
        {
            string value;
            foreach (var customerProperties in customer.GetType().GetProperties())
            {

                switch (customerProperties.Name) {
                    case "Email":
                        var emailPattern = @"^[^@\s]+@(gmail|hotmail|yahoo)\.(com|gr)$";
                        value = AnsiConsole.Prompt(
                        new TextPrompt<string>($"[bold green]Provide Customer {customerProperties.Name}:[/] ")
                        .DefaultValue<string>(customerProperties.GetValue(customer).ToString())
                        .Validate(input =>
                        {
                            if (!Regex.IsMatch(input,emailPattern))
                            {
                                return ValidationResult.Error("[red]Email should contain @,.com,.gr, and a mailing organization[/]");
                            }
                            return ValidationResult.Success();
                        }
                        ));
                        customer.Email = value;
                    break;

                    case "Phone":
                        value = AnsiConsole.Prompt(
                        new TextPrompt<string>($"[bold green]Provide Customer {customerProperties.Name}:[/] ")
                        .DefaultValue<string>(customerProperties.GetValue(customer).ToString())
                        .Validate(input => 
                        {
                            if ((input.Length != 10) || (!input.All(char.IsDigit)))
                                return ValidationResult.Error("[red]Phone Number should contain 10 Digits[/]");
                            
                            if (! input.StartsWith("69"))
                                return ValidationResult.Error("[red]Phone Number should start with 69[/]");
                            
                            return ValidationResult.Success();
                        }
                        ));
                        customer.Phone = value;
                        break;
                }
            }
            return customer;
        }

        public static Appointment UpdateAppointmentFields(Appointment appointment)
        {
            foreach (var appointmentProperties in appointment.GetType().GetProperties())
            {   
                if(Enum.TryParse(appointmentProperties.Name,true, out AppointmentFields appointmentField))
                switch (appointmentField) 
                {
                    case AppointmentFields.AppointmentDate:
                        string inputFormat = "dd/MM/yyyy h:mm:ss tt";
                        string? inputDate = appointmentProperties.GetValue(appointment)?.ToString();
                        inputDate = inputDate?.Replace("πμ","am").Replace("μμ","pm");
                        DateTime.TryParseExact(inputDate,inputFormat,CultureInfo.InvariantCulture, DateTimeStyles.None,out DateTime parsedDate);
                        DateTime updateDate = AnsiConsole.Prompt( new TextPrompt<DateTime>($"Provide Updated Date [red][[MM/DD/YYYY HH:MM]][/]")
                        .DefaultValue(parsedDate));
                        appointment.AppointmentDate = updateDate;
                        break;
                
                    case AppointmentFields.ServiceType:
                        string defaultAppointment = appointmentProperties.GetValue(appointment)?.ToString() ?? string.Empty;
                        Enum.TryParse(defaultAppointment, true, out ServiceTypeEnum defaultAppointmentType);
                        string opossiteAppointment = string.Empty;
                        
                        switch (defaultAppointmentType) 
                        {
                            case ServiceTypeEnum.PersonalTraining:
                                opossiteAppointment = "Massage";
                                break;
                        
                            case ServiceTypeEnum.Massage:
                                opossiteAppointment = "Personal Training";
                                break;
                        }
                        string? updateServiceType = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                .Title($"[bold green]Please Provide Service Type[/]\n")
                                .PageSize(3)
                                .AddChoices([defaultAppointment!,opossiteAppointment]));
                        
                        if (updateServiceType is not null)
                            appointment.ServiceType = updateServiceType;
                        else
                            throw new InvalidOperationException("No Valid Service type Selected");
                        break;
                
                    case AppointmentFields.AppointmentNotes:
                        string? defaultAppointmentNotes = appointmentProperties.GetValue(appointment)?.ToString();
                        string? updatedAppointmentNotes = AnsiConsole.Prompt(new TextPrompt<string>("Provide the Updated Appointment Notes")
                        .DefaultValue(defaultAppointmentNotes!)
                        );
                        appointment.AppointmentNotes = updatedAppointmentNotes;
                        break;
                }

            }
            return appointment;
        }

        public static void ReadAppointmentsData(ReadOnlyCollection<Appointment> appointmentsList)
        {
            //Table appointmentTable = AppointmentCustomerDataTable();
            string color = "blue";
            Table appointmentTable = CreateDataTable(["Id","Full Name","Service Type","Appointment Date","Appointment Notes"]);
            foreach (Appointment appointment in appointmentsList.AsReadOnly())
            {
                appointmentTable.AddRow(
                    new Markup($"[{color}]{appointment.Id}[/]"),
                    new Markup($"[{color}]{appointment.Customer.Name}[/]"),
                    new Markup($"[{color}]{appointment.ServiceType}[/]"),
                    new Markup($"[{color}]{appointment.AppointmentDate}[/]"),
                    new Markup($"[{color}]{appointment.AppointmentNotes}[/]")
                    );

            }
            AnsiConsole.Write(appointmentTable);
        }


        public static void ReadCustomerData(ReadOnlyCollection<Customer> customersList)
        {
            string color = "blue";
            Table customerTable = CreateDataTable(["Id","Full Name","Email","Phone"]);
            foreach (Customer customers in customersList.AsReadOnly())
            {
                customerTable.AddRow(
                    new Markup($"[{color}]{customers.Id}[/]"),
                    new Markup($"[{color}]{customers.Name}[/]"),
                    new Markup($"[{color}]{customers.Email}[/]"),
                    new Markup($"[{color}]{customers.Phone}[/]")
                    );

            }
            AnsiConsole.Write(customerTable);
        }

    }

}