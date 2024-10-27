# bookrentalprojectcode
Book Rental Service Project

Project Link : https://github.com/ParthiBanGitAcc/bookrentalprojectcode.git

1) Open `appsettings.json` file and update connection string's `data source=your server name`
      "ConnectionStrings": {
  "DefaultConnection": "Server=LAPTOP-94RTSUV3\\SQLEXPRESS;Database=BookRentalDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
2. Delete `Migrations` folder
3. Open Tools > Package Manager > Package manager console
4. Run these 2 commands
       (i) dotnet ef migrations add InitialCreate    
       (ii) dotnet ef database update
5) Necessary and sample scripts are attached in DB Scripts folder
6) Now you can run this project

Completed Requirements:
1.	Search for Books by Name and/or Genre
2.	Rent a Book
3.	Return a Book
4.	View Rental History
5.	Mark Overdue Rentals: Automatically mark rentals as overdue if not returned within 2 weeks.
6.	Email Notifications: Send an email notification to users when their rentals become overdue.
7.	Stats to show the most overdue book, most popular and least popular
8.	Implement comprehensive error and validations.
9.	Implement basic logging to capture system activity (e.g., rental events, errors).

Below things are used in project:
1. Used an “errorMessages.json” file to store custom display messages.
2. Utilized Serilog to capture logs.
3. Configured logging services to capture logs in both Debug and Console.
4. Set up an SMTP server for sending emails.
5. Integrated Swagger for posting data and testing endpoints.
6. Implemented EntityFrameworkCore.
7. Added Exception Handling Middleware to catch unhandled exceptions in the project.

