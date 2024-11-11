# Assignment 2
## Appointment Management System

### Overview
A new wellness and fitness center requires an application to manage customer appointments for its services. The application should assist staff in easily handling bookings, updates, cancellations, and viewing schedules. Your task is to develop a simple console application using the latest version of .NET.

The application should enable users to perform CRUD (Create, Read, Update, Delete) operations on appointment data. Each appointment will be associated with the center's customers, who should also be managed by the staff. The console application will guide users to choose between performing CRUD operations on customer data or appointment data.

### Features
#### Customer Operations
##### 1. Create Customer
Staff should keep the following data for the customer: Name, Email, Phone Number.
##### 2. Read Customers
Retrieve and display the list of all customers from the data store.
##### 3. Update Customer
Update the customer's details with the new information.
##### 4. Delete Customer
Delete a customer from the data store.

#### Appointments Operations
Users of the application are the staff of the center. Users should perform the following actions
##### 1. Create Appointment
Staff will be able to add new appointments. For the creation of the appointment, users should relate the appointment with an existing customer. Also, include service type, date, time, and optional notes for the booking. The services that the wellness and fitness center provides are Massage and Personal Training. Each appointment should have a unique identifier.
##### 2. Cancel/Remove appointment
Staff could remove an existing appointment.
##### 3. Update appointment
Staff could change the info of an existing appointment.Staff could change the info of an the existing appoitment
##### 4. Read Appointments
Users could view all the existing appointments as a report list.

#### Data Persistence:
Implement data persistence to store appointments & customers by using simple List objects to store them in memory.

#### User Interface:
Since this is a console application, ensure the user interface is intuitive and easy to navigate. Use menus and prompts to guide users through different operations.

#### Validation:
mplement basic input validation to ensure that users provide valid data when using the application and creating or updating customer and appointment records. For example, ensure email addresses are in the correct format, and phone numbers have the right number of digits, etc.

#### Additional Considerations:
* Download **visual studio community edition** in order to implement the console app (https://visualstudio.microsoft.com/)
* Submit your project to your existing github repository.
* Add a new branch **feature/assignment2** and place your project to the folder **src/Appointments.Console**.
* Create a pull request in order the codebase will be reviewed by Collaborators.
* Folders structure
  ```
  ├── aa-external-coaching
  │   ├── docs
  │   │   ├── cv.md
  │   ├── src
  │   │   ├── Appointments.Console  
  └─── Readme.md
  ```
* As an optional improvement, consider using **Spectre.Console** to enhance the user interface of your console application. Spectre.Console is a powerful .NET library that provides rich text formatting, tables, trees, progress bars, and many other interactive console elements.For more details and documentation, visit the official page: [Spectre.Console Documentation](https://spectreconsole.net/).


