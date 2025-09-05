# 🏦 MiniBank - Full-Stack ASP.NET Core Banking Portal  

MiniBank is a complete web application built to simulate a **basic banking credit application portal**.  
Users can register, submit detailed credit applications (with file uploads), and track their status.  
A secure employee portal allows bank staff to review applications and approve/reject them.  

This project demonstrates **ASP.NET Core MVC with clean architecture** and **SQL Server (Stored Procedures)** for a robust backend.  

---

## 🚀 Features  

### 🔐 Secure User Authentication  
- Full registration, login, and logout functionality.  
- Passwords securely hashed with **BCrypt**.  

### 👥 Role-Based Access Control  
- **User Role**: Submit new applications, view history, track status (Pending, Approved, Rejected).  
- **Employee Role**: View all applications, inspect details, update status.  

### 📝 Comprehensive Application Form  
- Detailed business and banking information.  
- Image upload for company logos.  
- **Validation**: Client-side (jQuery) + server-side (C#).  

### 🎨 Dynamic UI  
- Dependent dropdowns (State → City) with **AJAX & JSON**.  
- User-friendly feedback with color-coded status badges.  

### 📊 Application Management  
- Users: Delete their own **Pending** applications.  
- Employees: Delete **any** application.  

---

## 🛠️ Tech Stack & Architecture  

| Category      | Technology / Concept |
|---------------|----------------------|
| **Backend**   | C# (.NET 8), ASP.NET Core MVC, Cookie Auth, BCrypt.Net-Next |
| **Frontend**  | HTML, CSS, Bootstrap 5, JavaScript, jQuery, Razor Views |
| **Database**  | Microsoft SQL Server, Stored Procedures, Raw ADO.NET |
| **Architecture** | MVC, ViewModels, Service Layer (`LocationService.cs`) |
| **Data Format** | JSON (for dynamic dropdowns) |

---

## 🖥️ Getting Started  

### ✅ Prerequisites  
- .NET 8 SDK  
- Visual Studio 2022 (with **ASP.NET and web development** workload)  
- SQL Server + SQL Server Management Studio (SSMS)  

### 📦 Installation Steps  

1. **Clone the Repository**  
   ```bash
   git clone https://github.com/Abhishek2077/MiniBank.git


### 2. Set Up the Database
1. Open **SQL Server Management Studio (SSMS)** and connect to your local SQL instance.  
2. Open the `setup.sql` file from the project in SSMS.  
3. Execute the entire script.  
   - This will create the **MiniBankDB** database, all required tables, and all stored procedures.  

---

### 3. Configure the Connection String
1. In **Visual Studio Solution Explorer**, find the file `appsettings.Example.json`.  
2. Make a copy of it and rename the copy to **appsettings.json**.  
3. Open the new `appsettings.json` and update the `DefaultConnection` string with your local SQL Server name.  

---

### 4. Run the Project
1. Open the `MiniBank.sln` file in Visual Studio.  
2. The required **NuGet packages** (e.g., `BCrypt`) will restore automatically.  
   - If not, right-click the solution and select **"Restore NuGet Packages"**.  
3. Press **F5** or click the **Start** button to launch the application.  
4. The website will open in your browser.  

---

### 5. Register and Use
- Register a new **User** account.  
- To create an **Employee** account:  
  1. Register a second account.  
  2. Open SSMS and update its role in the `Bank_Users` table from `"User"` to `"Employee"`.  

---

# 📁 Project Structure (ASP.NET Core MVC)

This project follows the **standard ASP.NET Core MVC structure** to ensure a clean separation of concerns.

---

## 📂 Controllers/
Contains the **C# "brain" of the application**. Controllers handle user requests, interact with the database, and decide which view to render.

- **AccountController.cs** → Handles user registration and login logic.  
- **DashboardController.cs** → Core controller for both **User** and **Employee** dashboards, handling form submissions and application management.

---

## 📂 Models/
C# classes that define the **data structure** of the application.

- **Data Models** (e.g., `BankUser.cs`, `CreditApplication.cs`) → Directly represent database tables.  
- **ViewModels** (e.g., `UserDashboardViewModel.cs`, `EmployeeApplicationViewModel.cs`) → Custom classes designed to shape data perfectly for a specific view.

---

## 📂 Views/
Contains the **.cshtml files** that generate the HTML pages shown to users.

---

## 📂 Services/
Helper classes that store **business logic**, keeping controllers clean.

- **LocationService.cs** → Manages fetching **states and cities** for dynamic dropdowns.

---

## 📂 wwwroot/
Holds all **static front-end assets**, including:
- CSS  
- JavaScript files  
- Uploaded images  

---

## 📂 Database/
- **setup.sql** → Complete script to create and configure the database from scratch.

---

## 📄 Program.cs
The **entry point** of the application:
- Configures services like **authentication** and **database connections**.  
- Launches the application.

---


