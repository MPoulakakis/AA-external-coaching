using System.Collections.ObjectModel;
using AppointmentManagementSystem.Data.Abstractions;
using AppointmentManagementSystem.Data.Enums;
using AppointmentManagementSystem.Data.Models;
using AppointmentManagementSystem.ServicesHelper;

namespace AppointmentManagementSystem.Data.Repositories;

public class InMemoryAppointmentRepository<T> : IAppointmentRepository<T> where T : Appointment
{
    private readonly List<T> _appointments = InitialData.GetInitialAppointments<T>();//[];  //InitialData.GetInitialAppointments<T>();
    public Task CreateAppointment(T appointment)
    {
        if (appointment is not null)
        {
            _appointments.Add(appointment);
        }
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAppointment(int id)
    {
        bool isDeleted = false;
        var foundAppointment = _appointments.Find(x => x.Id == id);
        isDeleted = foundAppointment is not null && _appointments.Remove(foundAppointment);
        return Task.FromResult(isDeleted);
    }

    public Task<ReadOnlyCollection<T>> GetAppointments() => Task.FromResult(_appointments.AsReadOnly());

    public Task UpdateAppointment(T appointment)
    {
        var foundAppointment = _appointments.Find(x => x.Id == appointment.Id);
        if (foundAppointment is not null)
        {
            //foundAppointment.ServiceType = appointment.ServiceType;
            foundAppointment.AppointmentDate = appointment.AppointmentDate;
            foundAppointment.AppointmentNotes = appointment.AppointmentNotes;
        }
        return Task.CompletedTask;
    }

    public Task<T> AppointmentExists(int id)
    {
        var foundAppointment = _appointments.Find(x => x.Id == id);
        if (foundAppointment is not null)
            return Task.FromResult(foundAppointment);
        else
            return Task.FromResult<T>(null);
    }
    
    public Task<int> GetAppointmentsCount<TAppointment>() where TAppointment : Appointment
    {
        var appointmentCount = _appointments.Count(ap => ap is TAppointment);
        return Task.FromResult(appointmentCount);
    }

    public Task<int> GetEmployeeGenderPreference()
    {
        int femaleMassauer = _appointments.MassageAppointmentByGender(EmployeeGender.Female);
        int maleMassauer = _appointments.MassageAppointmentByGender(EmployeeGender.Male);
        
        return Task.FromResult(
            (femaleMassauer > maleMassauer) ? 0 :
            (femaleMassauer < maleMassauer) ? 1 : 2
            );
    }

    public Task<List<TrainingDuration>> GetTrainingDuration()
    {
        List<TrainingDuration> preferedDuration = _appointments.PersonalTrainingAppointmentByDuration().ToList();
        return Task.FromResult(preferedDuration);
    }

    public Task<List<DateTime>> GetPreferedMassageDates()
    {
                
        int massageMaxCount = _appointments
        .OfType<MassageAppointment>()
        .GroupBy(ap => ap.AppointmentDate.Date)
        .Max(group => group.Count());

        List<DateTime> massageMaxCountDate = _appointments
        .OfType<MassageAppointment>()
        .GroupBy(group => group.AppointmentDate.Date)
        .Where(group => group.Count() == massageMaxCount)
        .Select(group => group.Key).ToList();

        return Task.FromResult(massageMaxCountDate);
    }

    public Task<List<DateTime>> GetPreferedPersonalDates()
    {        
        int personalMaxCount = _appointments
        .OfType<PersonalTrainingAppointment>()
        .GroupBy(ap => ap.AppointmentDate.Date)
        .Max(group => group.Count());

        List<DateTime> personalMaxCountDate = _appointments
        .OfType<PersonalTrainingAppointment>()
        .GroupBy(group => group.AppointmentDate.Date)
        .Where(group => group.Count() == personalMaxCount)
        .Select(group => group.Key).ToList();
    
        return Task.FromResult(personalMaxCountDate);
    }

    public Task<List<MassageType>> GetPreferedMassageType()
    {
        var maxMassageTypeCount =  _appointments
        .GroupMassageAppointmentByType()
        .Max(group => group.Count());

        var massageTypes = _appointments
        .GroupMassageAppointmentByType()
        .Where(group => group.Count() == maxMassageTypeCount)
        .Select(group => group.Key).ToList();    
    
        return Task.FromResult(massageTypes);
    }

    public Task<List<DateTime>> GetDateWithMostAppointments()
    {
        var maxAppointments = _appointments
        .GroupBy(ap => ap.AppointmentDate.Date)
        .Max(group => group.Count());

        var maxAppointmentsDate = _appointments
        .GroupBy(ap => ap.AppointmentDate.Date)
        .Where(group => group.Count() == maxAppointments)
        .Select(group => group.Key).ToList();

        return Task.FromResult(maxAppointmentsDate);
    }

    public Task<List<DateTime>> GetDateWithLeastAppointments()
    {
        var minAppointments = _appointments
        .GroupBy(ap => ap.AppointmentDate.Date)
        .Min(group => group.Count());

        var minAppointmentsDate = _appointments
        .GroupBy(ap => ap.AppointmentDate.Date)
        .Where(group => group.Count() == minAppointments)
        .Select(group => group.Key).ToList();

        return Task.FromResult(minAppointmentsDate);
    }

}
