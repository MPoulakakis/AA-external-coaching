using AppointmentManagementSystem.Data.Enums;
using AppointmentManagementSystem.Data.Models;

namespace AppointmentManagementSystem.Data
{
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
            MassageAppointment massageAp1 = new(GetInitialCustomers()[0],MassageType.Reflexology,EmployeeGender.Male,new DateTime(2024, 11, 30, 10, 0, 0),"Reflexology Massage Notes");
            MassageAppointment massageAp2 = new(GetInitialCustomers()[1],MassageType.HotStone,EmployeeGender.Female,new DateTime(2024, 12, 02, 13, 0, 0),"HotStone Massage Notes");
            MassageAppointment massageAp3 = new(GetInitialCustomers()[2],MassageType.Relaxing,EmployeeGender.Male,new DateTime(2024, 12, 05, 17, 0, 0),"Relaxing Massage Notes");

            PersonalTrainingAppointment personalAp1 = new(GetInitialCustomers()[0], trainingDuration.ThirtyMinutes, new DateTime(2024, 11, 15, 20, 0, 0),"Legs","Right Elbow Injury", "Deep tissue massage");
            PersonalTrainingAppointment personalAp2 = new(GetInitialCustomers()[1], trainingDuration.SixtyMinutes, new DateTime(2024, 11, 18, 12, 0, 0));
            PersonalTrainingAppointment personalAp3 = new(GetInitialCustomers()[2], trainingDuration.NinetyMinutes, new DateTime(2024, 11, 27, 15, 0, 0),"Biceps","Knee Injury", "Biceps Training");
            var initialAppointmentData = new List<Appointment>
            {
                massageAp1,
                massageAp2,
                massageAp3,
                personalAp1,
                personalAp2,
                personalAp3
            };
            return initialAppointmentData.Cast<TAppointment>().ToList() ;
        }
    }
    
}