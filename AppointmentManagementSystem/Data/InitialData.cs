using AppointmentManagementSystem.Data.Enums;
using AppointmentManagementSystem.Data.Models;

namespace AppointmentManagementSystem.Data;

public class InitialData()
{
    public static List<Customer> GetInitialCustomers()
    {
        var cs1 = new Customer(name: "Manos Poulakakis", email: "manolispoulakakis@gmail.com", phone: "6984153487");
        var cs2 = new Customer("Fenia Giannakopoulou", "feniagiannakopoulou@gmail.com", "6943254612");
        var cs3 = new Customer("Kostas Poulakakis", "kospoul@gmail.com", "6982806297");

        return [cs1,cs2,cs3];
    }
  
    public static List<TAppointment> GetInitialAppointments<TAppointment>() where TAppointment : Appointment
    {
        var cs = GetInitialCustomers();
        MassageAppointment massageAp1 = new(cs[0],MassageType.Reflexology,EmployeeGender.Male,new DateTime(2024, 11, 30, 10, 0, 0),"Reflexology Massage Notes");
        MassageAppointment massageAp2 = new(cs[1],MassageType.HotStone,EmployeeGender.Female,new DateTime(2024, 12, 02, 13, 0, 0),"HotStone Massage Notes");
        MassageAppointment massageAp3 = new(cs[2],MassageType.Relaxing,EmployeeGender.Male,new DateTime(2024, 12, 06, 17, 0, 0),"Relaxing Massage Notes");
        MassageAppointment massageAp4 = new(cs[0],MassageType.Relaxing,EmployeeGender.Male,new DateTime(2024, 12, 06, 18, 0, 0),"Relaxing Massage Notes");
        MassageAppointment massageAp5 = new(cs[1],MassageType.HotStone,EmployeeGender.Female,new DateTime(2024, 12, 05, 13, 0, 0),"Relaxing Massage Notes");
        MassageAppointment massageAp6 = new(cs[0],MassageType.HotStone,EmployeeGender.Female,new DateTime(2024, 12, 05, 14, 0, 0),"Relaxing Massage Notes");
        MassageAppointment massageAp7 = new(cs[1],MassageType.Relaxing,EmployeeGender.Male,new DateTime(2024, 12, 05, 11, 0, 0),"Relaxing Massage Notes");
        MassageAppointment massageAp8 = new(cs[2],MassageType.Reflexology,EmployeeGender.Female,new DateTime(2024, 12, 05, 12, 0, 0),"Relaxing Massage Notes");

        PersonalTrainingAppointment personalAp1 = new(cs[0], TrainingDuration.ThirtyMinutes, new DateTime(2024, 11, 15, 20, 0, 0),"Legs","Right Elbow Injury", "Deep tissue massage");
        PersonalTrainingAppointment personalAp2 = new(cs[1], TrainingDuration.SixtyMinutes, new DateTime(2024, 11, 18, 12, 0, 0));
        PersonalTrainingAppointment personalAp3 = new(cs[2], TrainingDuration.NinetyMinutes, new DateTime(2024, 11, 27, 15, 0, 0),"Biceps","Knee Injury", "Biceps Training");
        PersonalTrainingAppointment personalAp4 = new(cs[1], TrainingDuration.SixtyMinutes, new DateTime(2024, 11, 27, 15, 0, 0),"Biceps","Knee Injury", "Biceps Training");
        PersonalTrainingAppointment personalAp5 = new(cs[2], TrainingDuration.ThirtyMinutes, new DateTime(2024, 11, 28, 15, 0, 0),"Biceps","Knee Injury", "Biceps Training");
        PersonalTrainingAppointment personalAp6 = new(cs[0], TrainingDuration.ThirtyMinutes, new DateTime(2024, 11, 27, 15, 0, 0),"Biceps","Knee Injury", "Biceps Training");
        PersonalTrainingAppointment personalAp7 = new(cs[1], TrainingDuration.SixtyMinutes, new DateTime(2024, 11, 28, 16, 30, 0),"Biceps","Knee Injury", "Biceps Training");
        PersonalTrainingAppointment personalAp8 = new(cs[2], TrainingDuration.NinetyMinutes, new DateTime(2024, 11, 27, 16, 0, 0),"Biceps","Knee Injury", "Biceps Training");
        PersonalTrainingAppointment personalAp9 = new(cs[1], TrainingDuration.NinetyMinutes, new DateTime(2024, 11, 28, 15, 30, 0),"Biceps","Knee Injury", "Biceps Training");
        var initialAppointmentData = new List<Appointment>
        {
            massageAp1,
            massageAp2,
            massageAp3,
            massageAp4,
            massageAp5,
            massageAp6,
            massageAp7,
            massageAp8,
            personalAp1,
            personalAp2,
            personalAp3,
            personalAp4,
            personalAp5,
            personalAp6,
            personalAp7,
            personalAp8,
            personalAp9,

        };
        return initialAppointmentData.Cast<TAppointment>().ToList() ;
    }
}

