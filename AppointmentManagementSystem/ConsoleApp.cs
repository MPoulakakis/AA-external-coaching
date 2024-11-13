using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Globalization;
using AppointmentManagementSystem.Data.Models;
using Spectre.Console;
using AppointmentManagementSystem.Data.Enums;

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
        
        public static int DurationSelector(int[] TrainingDuration)
        {
            int trainingDuration = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
            .Title($"[bold green]Select Session Duration[/]\n")
            .PageSize(4)
            .AddChoices(TrainingDuration));
            return trainingDuration;
        }

        public static TEnum EnumSelector<TEnum>(string Title,string[] Choices) where TEnum: struct, Enum
        {
            string appointmentTypeSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"[bold green]{Title}[/]\n")
            .PageSize(5)
            .AddChoices(Choices));

            appointmentTypeSelection = appointmentTypeSelection.Replace(" ","");
            Enum.TryParse(appointmentTypeSelection, true, out TEnum appointmentType);
            return appointmentType;
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


        public static Appointment UpdateAppointmentFields<T>( T appointment) where T : Appointment
        {
            if (appointment is PersonalTrainingAppointment personalTrainingList)
            {
                foreach(var appointmentProperties in appointment.GetType().GetProperties())
                {
                    if(Enum.TryParse(appointmentProperties.Name,true, out AppointmentFields appointmentField))
                    switch (appointmentField) 
                    {
                        case AppointmentFields.AppointmentDate:
                            string inputFormat = "dd/MM/yyyy h:mm:ss tt";
                            string? inputDate = appointmentProperties.GetValue(appointment)?.ToString();
                            DateTime updateDate = UpdateAppointmentDate(inputDate,inputFormat);
                            personalTrainingList.AppointmentDate = updateDate;
                            break;
                    
                        case AppointmentFields.AppointmentNotes:
                            string? defaultAppointmentNotes = appointmentProperties.GetValue(appointment)?.ToString();
                            string? updatedAppointmentNotes = AnsiConsole.Prompt(new TextPrompt<string>("Provide the Updated Appointment Notes")
                                .DefaultValue(defaultAppointmentNotes!)
                            );
                            personalTrainingList.AppointmentNotes = updatedAppointmentNotes;
                            break;
                    
                        case AppointmentFields.Duration:
                            var duration = ConsoleApp.EnumSelector<trainingDuration>("Select Training Duration",["Thirty Minutes","Sixty Minutes", "Ninety Minutes"]);
                            personalTrainingList.Duration = duration;
                            break;

                        case AppointmentFields.FocusedTraining:
                            string focusedTraining  = ConsoleApp.CliTextPrompt("Focused Muscle Group(s)", isOptional:true);
                            personalTrainingList.FocusedTraining = focusedTraining;
                            break;

                        case AppointmentFields.InjuriesComments:
                            string injuriesComments = ConsoleApp.CliTextPrompt("Injury Report", isOptional:true);
                            personalTrainingList.InjuriesComments = injuriesComments;
                            break;
                    }
                    
                }   
                return personalTrainingList;
            }
            else if (appointment is MassageAppointment massageAppointment)
            {
                foreach (var appointmentProperties in appointment.GetType().GetProperties())
                {
                    if(Enum.TryParse(appointmentProperties.Name,true, out AppointmentFields appointmentField))
                    switch (appointmentField) 
                    {
                        case AppointmentFields.AppointmentDate:
                            string inputFormat = "dd/MM/yyyy h:mm:ss tt";
                            string? inputDate = appointmentProperties.GetValue(appointment)?.ToString();
                            DateTime updateDate = UpdateAppointmentDate(inputDate,inputFormat);
                            massageAppointment.AppointmentDate = updateDate;
                            break;
                    
                        case AppointmentFields.AppointmentNotes:
                            string? defaultAppointmentNotes = appointmentProperties.GetValue(appointment)?.ToString();
                            string? updatedAppointmentNotes = AnsiConsole.Prompt(new TextPrompt<string>("Provide the Updated Appointment Notes")
                                .DefaultValue(defaultAppointmentNotes!)
                            );
                            massageAppointment.AppointmentNotes = updatedAppointmentNotes;
                            break;
                    
                        case AppointmentFields.MassageType:
                            MassageType massageType = EnumSelector<MassageType>("Select Massage Type",["Relaxing","Reflexology","Hot Stone"]);
                            massageAppointment.MassageType = massageType;
                            break;

                        case AppointmentFields.EmployeeGender:
                            EmployeeGender employeeSex = EnumSelector<EmployeeGender>("Select Employee Sex",["Male","Female"]);
                            massageAppointment.EmployeeSex = employeeSex;
                            break;
                    }
                   
                }
                return massageAppointment;
            }
            else
                return appointment;
        }

        public static void ReadAppointmentsData<T>(ReadOnlyCollection<T> appointmentsList) where T : Appointment
        {
            Table massageTable = new();
            Table personalTable = new();
            string color = "blue";
            massageTable = CreateDataTable(["Id","Full Name","Employee Gender","Massage Type","Appointment Date","Appointment Notes"]);
            personalTable = CreateDataTable(["Id","Full Name","Training Duration","Appointment Date","Training Focus","Injuries","Appointment Notes"]);
            foreach (var appointment in appointmentsList)
            {
                if(appointment is MassageAppointment massageAppointment)
                {
                    massageTable.AddRow(
                        new Markup($"[{color}]{massageAppointment.Id}[/]"),
                        new Markup($"[{color}]{massageAppointment.Customer.Name}[/]"),
                        new Markup($"[{color}]{AddSpacesToEnums(massageAppointment.EmployeeSex)}[/]"),
                        new Markup($"[{color}]{massageAppointment.MassageType}[/]"),
                        new Markup($"[{color}]{massageAppointment.AppointmentDate}[/]"),
                        new Markup($"[{color}]{massageAppointment.AppointmentNotes}[/]")
                    );

                }
                else if (appointment is PersonalTrainingAppointment personalAppointment)
                {
                    personalTable.AddRow(
                        new Markup($"[{color}]{personalAppointment.Id}[/]"),
                        new Markup($"[{color}]{personalAppointment.Customer.Name}[/]"),
                        new Markup($"[{color}]{AddSpacesToEnums(personalAppointment.Duration)}[/]"),
                        new Markup($"[{color}]{personalAppointment.AppointmentDate}[/]"),
                        new Markup($"[{color}]{personalAppointment.FocusedTraining}[/]"),
                        new Markup($"[{color}]{personalAppointment.InjuriesComments}[/]"),
                        new Markup($"[{color}]{personalAppointment.AppointmentNotes}[/]")
                    );
                }
            }
            massageTable.ShowRowSeparators();
            personalTable.ShowRowSeparators();
            AnsiConsole.Write(massageTable);
            AnsiConsole.Write(personalTable);
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

        public static string AddSpacesToEnums(Enum value)
        {
            string enumString = value.ToString();
            string formattedString = Regex.Replace(enumString,"(\\B[A-Z])"," $1");
            return formattedString;

        }

        public static DateTime UpdateAppointmentDate(string? inputDate, string inputFormat)
        {
            inputDate = inputDate?.Replace("πμ","am").Replace("μμ","pm");
            DateTime.TryParseExact(inputDate,inputFormat,CultureInfo.InvariantCulture, DateTimeStyles.None,out DateTime parsedDate);
            DateTime updateDate = AnsiConsole.Prompt( new TextPrompt<DateTime>($"Provide Updated Date [red][[MM/DD/YYYY HH:MM]][/]")
            .DefaultValue(parsedDate));
            return updateDate;
        }

    }

}