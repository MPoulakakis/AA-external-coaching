namespace AppointmentManagementSystem
{
    class Appointment
    {
        Customer customer;
        private string serviceType;
        public string ServiceType
        {
            get { return serviceType; }
            set { serviceType = value; }
        }

        private DateTime appointmentDate;
        public DateTime AppointmentDate
        {
            get { return appointmentDate; }
            set { appointmentDate = value; }
        }

        private string appointmentNotes;
        public string AppointmentNotes
        {
            get { return appointmentNotes; }
            set { appointmentNotes = value; }
        }
        

        public Appointment(Customer customer, string serviceType, DateTime appointmentDate , string appointmentNotes = "")
        {
            this.customer = customer;
            ServiceType = serviceType;
            AppointmentDate = appointmentDate;
            AppointmentNotes = appointmentNotes;
        }
        
        
        
    }
}