using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AppointmentManagementSystem.Data.Enums;
using AppointmentManagementSystem.Data.Models;
using Spectre.Console;

namespace AppointmentManagementSystem;

public class ConsoleApp2
{
    public string MainColor { get;} = "bold blue";
    public string FillerColor { get;} = "bold grey100";
    public string Error { get;} = "bold red";


    public void DisplayToUser(string messageUser,string textColor = "green")
    {
        AnsiConsole.Markup($"[{textColor}]{messageUser}[/]\n");
    }
    public void DisplayMaxAppointmentDate<TAppointment>(List<DateTime> dates) where TAppointment : Appointment
    {
        string appointmentName= Regex.Replace(typeof(TAppointment).Name, "(\\B[A-Z])", " $1") + "s";
        if (dates.Count != 0)
        {
            DisplayToUser($"Date(s) With Most {appointmentName}",MainColor);
            foreach (var date in dates)
                DisplayToUser($"{date.ToShortDateString()} ({date.DayOfWeek})",FillerColor);
        }        
        else
            DisplayToUser($"No {appointmentName} Found ",Error);
    }
    
    public void DisplayMinAppointmentDate<TAppointment>(List<DateTime> dates) where TAppointment : Appointment
    {
        string appointmentName= Regex.Replace(typeof(TAppointment).Name, "(\\B[A-Z])", " $1") + "s";
        if (dates.Count != 0)
        {
            DisplayToUser($"Date(s) With Least {appointmentName}",MainColor);
            foreach (var date in dates)
                DisplayToUser($"{date.ToShortDateString()} ({date.DayOfWeek})",FillerColor);
        }        
        else 
            DisplayToUser($"No {appointmentName} Found",Error);
    }

    public void DisplayMassageTypePreference(List<MassageType> massageType)
    {
        if (massageType.Count != 0)
        {
            DisplayToUser("Prefered Massaged Type(s)",MainColor);
            foreach (var type in massageType )
            {
                DisplayToUser($"{type}",FillerColor);
            }
        }
        else
            DisplayToUser($"No Appointment Found", Error);
    }
    public void DisplayEmployeGenderPreference(int preference)
    {
        if (preference == 2)
            DisplayToUser($"Users Prefer Both [{FillerColor}]Male[/] and [{FillerColor}]Female[/] Massauer Equally",MainColor);
        else
            DisplayToUser($"Users Prefer [{FillerColor}]{(EmployeeGender)preference}[/] Massauer",MainColor);
    }

    public void TrainingDurationPreference(List<TrainingDuration> trainingDurations)
    {
        if (trainingDurations.Count != 0)
        {
            DisplayToUser("Prefered Massaged Type(s)",MainColor);
            foreach (var type in trainingDurations )
            {
                DisplayToUser($"{type}",FillerColor);
            }
        }
        else
            DisplayToUser($"No Appointment Found", Error);
    }
}