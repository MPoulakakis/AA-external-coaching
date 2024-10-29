using System.ComponentModel;
using System.Data.Common;
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

        public static Table CreateCustomerDataTable()
        {
            var customerTable = new Table();
            customerTable.AddColumn(new TableColumn("[bold green]Full Name[/]").Centered());
            customerTable.AddColumn(new TableColumn("[bold green]Email[/]").Centered());
            customerTable.AddColumn(new TableColumn("[bold green]Phone Number[/]").Centered());

            return customerTable;
        }

        public static string CustomerFields(string customerField)
        {
            var field = AnsiConsole.Prompt(
                new TextPrompt<string>($"[bold green]Provide Customer {customerField}:[/] "));

            return field;
        }

        public static void ReadCustomerData(List<Customer> customersList)
        {
            Table customerTable = CreateCustomerDataTable();
            foreach (Customer customers in customersList)
            {
                customerTable.AddRow(
                    new Markup($"[magenta]{customers.Name}[/]"),
                    new Markup($"[magenta]{customers.Email}[/]"),
                    new Markup($"[magenta]{customers.Phone}[/]")
                    );

            }
            AnsiConsole.Write(customerTable);
        }

        public static bool CheckIfCustomerExists(string customerEmail, List<Customer> customersList)
        {
            bool isCustomerEmail = false;   
            foreach (Customer customer in customersList)
            {
                if (customerEmail == customer.Email)
                {
                    isCustomerEmail = true;
                    AnsiConsole.Markup("[green bold]Customer Found[/]\n");
                    Table customerTable = CreateCustomerDataTable();
                    customerTable.AddRow(new Markup($"[yellow]{customer.Name}[/]"), new Markup($"[yellow]{customer.Email}[/]"), new Markup($"[yellow]{customer.Phone}[/]"));
                    AnsiConsole.Write(customerTable);
                    return isCustomerEmail;
                }
            }
            if (! isCustomerEmail)
            {
                AnsiConsole.Markup("[red bold]Customer not Found[/]\n");
                return isCustomerEmail;
            }
            else
            {
                return isCustomerEmail;
            }

        }

        public static void UpdateCustomer(string customerEmail, List<Customer> customersList, string updateField, string updateValue)
        {
            bool customerFound = false;
            foreach (Customer customer in customersList)
            {
                if (customerEmail == customer.Email)
                {
                    customerFound = true ;
                    AnsiConsole.Markup("[green bold]Customer Found and will be[/] [red]Updated[/]\n");

                    switch(updateField)
                    {
                        case "Full Name":
                            customer.Name = updateValue;
                        break;

                        case "Email":
                            customer.Email = updateValue;
                        break;

                        case "Phone":
                            customer.Phone = updateValue;
                        break;
                    }
                    AnsiConsole.Markup("[green bold]Updated Customer[/]\n");
                    Table customerTable = CreateCustomerDataTable();
                    customerTable.AddRow(new Markup($"[bold red]{customer.Name}[/]"), new Markup($"[red]{customer.Email}[/]"), new Markup($"[red]{customer.Phone}[/]"));
                    AnsiConsole.Write(customerTable);
                    break;
                }
            }
            if (! customerFound)
            {
                AnsiConsole.Markup("[red bold]Customer not Found[/]\n");
            }   
        }

        public static void DeleteCustomer(string customerEmail, List<Customer> customersList)
        {
            bool customerFound = false;
            foreach (Customer customer in customersList)
            {
                if (customerEmail == customer.Email)
                {
                    customerFound = true;
                    AnsiConsole.Markup("[green bold]Customer Found and will be[/][red] Deleted[/]\n");
                    Table customerTable = CreateCustomerDataTable();
                    customerTable.AddRow(new Markup($"[bold red]{customer.Name}[/]"), new Markup($"[red]{customer.Email}[/]"), new Markup($"[red]{customer.Phone}[/]"));
                    AnsiConsole.Write(customerTable);
                    customersList.Remove(customer);
                    break;
                }
            }
            if (! customerFound)
            {
                AnsiConsole.Markup("[red bold]Customer not Found[/]\n");
            }

        }
    
    }

}