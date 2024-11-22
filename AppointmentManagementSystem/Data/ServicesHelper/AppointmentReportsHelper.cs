using System.Text.RegularExpressions;
using AppointmentManagementSystem.Data.Enums;
using AppointmentManagementSystem.Data.Models;
using Spectre.Console;

namespace AppointmentManagementSystem.ServicesHelper;


public static class AppointmentReportsHelper
{
    public static int MassageAppointmentByGender(this IEnumerable<Appointment> query , EmployeeGender employeeGender)
    {
        return query
        .OfType<MassageAppointment>()
        .Count(ap => ap.EmployeeSex == employeeGender);
    }
    public static IEnumerable<TrainingDuration> PersonalTrainingAppointmentByDuration(this IEnumerable<Appointment> query )
    {
        var maxCount = query
        .OfType<PersonalTrainingAppointment>()
        .GroupBy(ap => ap.Duration)
        .Max(group => group.Count());

        return query
        .OfType<PersonalTrainingAppointment>()
        .GroupBy(ap => ap.Duration)
        .Where(group => group.Count() == maxCount)
        .Select(group => group.Key);
    }

  
    public static int GetMaxGenderCount(this IEnumerable<Appointment> query)
    {
        return query
        .OfType<MassageAppointment>()
        .GroupBy(ap => ap.EmployeeSex)
        .Max(group => group.Count());
    }
    // GetMaxGenderCount and GetMaxDurationCount , probably could be collapsed together
    public static int GetMaxDurationCount(this IEnumerable<Appointment> query)
    {
        return query
        .OfType<PersonalTrainingAppointment>()
        .GroupBy(ap => ap.Duration)
        .Max(group => group.Count());
    }

    public static IEnumerable<IGrouping<MassageType, MassageAppointment>> GroupMassageAppointmentByType(this IEnumerable<Appointment> query)
    {
        return query
        .OfType<MassageAppointment>()
        .GroupBy(appointment => appointment.MassageType);
    }

}