
namespace AppointmentManagementSystem
{
    class Customer
    {
        private string name;
        private string email;
        private string phone;

        public string Name
        {
            get { return name; }
            set 
            { 
                name = value; 
            }
        }
        public string Email 
        {
            get{ return email; } 
            set
            {
                email = value;
            } 
        }
        public string Phone 
        {
            get{ return phone; } 
            set
            {
                phone = value;
            } 
        }                

        public Customer(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone; 

        }
   
    
    
    }
}