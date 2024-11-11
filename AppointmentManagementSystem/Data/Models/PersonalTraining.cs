namespace AppointmentManagementSystem.Data.Models;
public class PersonalTrainingAppointment(string focusedTraining, string injuriesComments  )
{
    public TimeSpan[] Duration { get;} = [new TimeSpan(0,30,0),new TimeSpan(0,1,0),new TimeSpan(1,30,0)];
    public string FocusedTraining { get; set; } = focusedTraining;
    public string InjuriesComments { get; set; } = injuriesComments;
}