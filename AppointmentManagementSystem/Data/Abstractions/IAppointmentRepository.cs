using System.Collections.ObjectModel;
using AppointmentManagementSystem.Data.Enums;
using AppointmentManagementSystem.Data.Models;

namespace AppointmentManagementSystem.Data.Abstractions;

public interface IAppointmentRepository<T> where T : Appointment
{
    Task<ReadOnlyCollection<T>> GetAppointments();
    Task CreateAppointment(T appointment);
    Task UpdateAppointment(T appointment);
    Task<bool> DeleteAppointment(int id);
    Task<T> AppointmentExists(int id);
    //Task<int> GetAppointmentsCount();
    Task<int> GetAppointmentsCount<TAppointment>() where TAppointment : Appointment;
    Task<int> GetEmployeeGenderPreference();
    Task<List<TrainingDuration>> GetTrainingDuration();
    Task<List<DateTime>> GetPreferedMassageDates();
    Task<List<DateTime>> GetPreferedPersonalDates();
    Task<List<MassageType>> GetPreferedMassageType();
    Task<List<DateTime>> GetDateWithMostAppointments();
    Task<List<DateTime>> GetDateWithLeastAppointments();
    
}