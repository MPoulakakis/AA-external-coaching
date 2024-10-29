using AppointmentManagementSystem;
using AppointmentManagementSystem.Utilities;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        // Render a table using Spectre.Console
        // var table = new Table();
        // table.AddColumn("Foo");
        // table.AddColumn("Bar");

        // table.AddRow("Baz", "[green]Qux[/]");

        // AnsiConsole.Write(table);
        string[] actionSelector = new string[5]
        {
            "Create",
            "Read",
            "Update",
            "Delete",
            "Exit"
        };

        string[] operationSelector = new string[3]
        {
            "Customer Data",
            "Appointment Data",
            "Exit"
        };

        string[] fieldUpdateSelector = new string[3]
        {
            "Full Name",
            "Email",
            "Phone"
        };


        
        List<Customer> customersList = new List<Customer>();
        List<Appointment> appointmentsList = new List<Appointment>();
        Customer ex1 = new Customer("Manos Poulakakis","manolispoulakakis@gmail.com","6984153487");
        Customer ex2 = new Customer("Fenia Giannakopoulou","feniagiannakopoulou@gmail.com","6943254612");
        Customer ex3 = new Customer("Kostas Poulakakis","kospoul@gmail.com","6982806297");

        customersList.Add(ex1);
        customersList.Add(ex2);
        customersList.Add(ex3);

        Appointment ap1 = new Appointment(ex1,"Massage",new DateTime(2024,11,15,20,0,0),"Deep tissue massage");
        Appointment ap2 = new Appointment(ex2,"Massage",new DateTime(2024,11,15,20,0,0));
        Appointment ap3 = new Appointment(ex3,"Massage",new DateTime(2024,11,15,20,0,0),"Full Body Training");

        appointmentsList.Add(ap1);
        appointmentsList.Add(ap2);
        appointmentsList.Add(ap3);
        

        string action = string.Empty;
        string operation;
        do
        {
        
            operation = Utilities.Selector(operationSelector,"Please Select Action to be executed or Exit to close the application");
            switch (operation)
            {
                case "Customer Data":
                    AnsiConsole.Markup($"[bold green]You Selected [red]{operation}[/][/]\n");       
                    do
                    {
                        action = Utilities.Selector(actionSelector,"Please Select Action to be executed or Exit to close the application");
                        switch (action)
                        {
                            case "Create":
                                Customer customer= new Customer(Utilities.CustomerFields("Full Name"), Utilities.CustomerFields("Email"), Utilities.CustomerFields("Phone"));
                                customersList.Add(customer);
                                break;

                            case "Read":
                                Utilities.ReadCustomerData(customersList);
                                break;

                            case "Update":
                                string updateEmail = Utilities.CliTextPrompt("Provide Customer Email");
                                bool isCustomer = Utilities.CheckIfCustomerExists(updateEmail, customersList);
                                if (isCustomer)
                                {
                                    string updateField = Utilities.Selector(fieldUpdateSelector,"Provide Customer Field That Needs To Be Upated");
                                    string updateValue = Utilities.CliTextPrompt($"Provide Updated {updateField}");
                                    Utilities.UpdateCustomer(updateEmail, customersList, updateField, updateValue);
                                }
                                break;

                            case "Delete":
                                string deleteEmail = Utilities.CliTextPrompt("Provide Customer Email");
                                // isCustomer = Utilities.CheckIfCustomerExists(deleteEmail, customersList);
                                Utilities.DeleteCustomer(deleteEmail, customersList);
                                break;

                            default:
                                AnsiConsole.Markup($"[bold red]Returning To Start Menu[/]\n");
                                break;
                        }
                    } while (action != "Exit");
                    break;

                case "Appointment Data":
                    AnsiConsole.Markup($"[bold green]You Selected [red]{operation}[/][/]\n");

                    do
                    {
                        action = Utilities.Selector(actionSelector,"Please Select Action to be executed or Exit to close the application");
                        switch (action)
                        {
                            case "Create":
                                break;
                            case "Read":
                                break;
                            case "Update":
                                break;
                            case "Delete":
                                break;
                            
                        }

                    } while (action != "Exit");

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
