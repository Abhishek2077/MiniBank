using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using MiniBank.Models;
using MiniBank.Services;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace MiniBank.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;

        public DashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            var locationService = new LocationService();
            ViewBag.States = locationService.GetStates();

            // Get current user ID
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var submittedApplications = new List<CreditApplication>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreditApplications_GetByUserID", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            submittedApplications.Add(new CreditApplication
                            {
                                ApplicationID = (int)reader["ApplicationID"],
                                UserID = (int)reader["UserID"],
                                CompanyStructure = reader["CompanyStructure"].ToString(),
                                BusinessSince = (DateTime)reader["BusinessSince"],
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                StreetAddress = reader["StreetAddress"].ToString(),
                                City = reader["City"].ToString(),
                                State = reader["State"].ToString(),
                                ZipCode = reader["ZipCode"].ToString(),
                                BankName = reader["BankName"]?.ToString(),
                                AccountNumber = reader["AccountNumber"]?.ToString(),
                                IFSCCode = reader["IFSCCode"]?.ToString(),
                                BankBranch = reader["BankBranch"]?.ToString(),
                                BankState = reader["BankState"]?.ToString(),
                                BankCity = reader["BankCity"]?.ToString(),
                                BankZipCode = reader["BankZipCode"]?.ToString(),
                                LogoImagePath = reader["LogoImagePath"]?.ToString(),
                                Status = reader["Status"].ToString(),
                                SubmittedAt = (DateTime)reader["SubmittedAt"]
                            });
                        }
                    }
                }
                connection.Close();
            }

            var viewModel = new UserDashboardViewModel
            {
                NewApplication = new CreditApplication(),
                SubmittedApplications = submittedApplications
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult Index(UserDashboardViewModel model, IFormFile logoFile)
        {
            var application = model.NewApplication;
            if (!ModelState.IsValid)
            {
                var locationService = new LocationService();
                ViewBag.States = locationService.GetStates();
                // Also get user's submitted applications for redisplay
                int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var submittedApplications = new List<CreditApplication>();
                string userConnectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(userConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("CreditApplications_GetByUserID", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserID", userId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                submittedApplications.Add(new CreditApplication
                                {
                                    ApplicationID = (int)reader["ApplicationID"],
                                    UserID = (int)reader["UserID"],
                                    CompanyStructure = reader["CompanyStructure"].ToString(),
                                    BusinessSince = (DateTime)reader["BusinessSince"],
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    StreetAddress = reader["StreetAddress"].ToString(),
                                    City = reader["City"].ToString(),
                                    State = reader["State"].ToString(),
                                    ZipCode = reader["ZipCode"].ToString(),
                                    BankName = reader["BankName"]?.ToString(),
                                    AccountNumber = reader["AccountNumber"]?.ToString(),
                                    IFSCCode = reader["IFSCCode"]?.ToString(),
                                    BankBranch = reader["BankBranch"]?.ToString(),
                                    BankState = reader["BankState"]?.ToString(),
                                    BankCity = reader["BankCity"]?.ToString(),
                                    BankZipCode = reader["BankZipCode"]?.ToString(),
                                    LogoImagePath = reader["LogoImagePath"]?.ToString(),
                                    Status = reader["Status"].ToString(),
                                    SubmittedAt = (DateTime)reader["SubmittedAt"]
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                var viewModel = new UserDashboardViewModel
                {
                    NewApplication = application,
                    SubmittedApplications = submittedApplications
                };
                return View(viewModel);
            }

            if (logoFile != null && logoFile.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + logoFile.FileName;
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                Directory.CreateDirectory(uploadsFolder);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    logoFile.CopyTo(fileStream);
                }
                application.LogoImagePath = "/uploads/" + uniqueFileName;
            }

            application.UserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            application.Status = "Pending";

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreditApplications_Insert", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", application.UserID);
                    command.Parameters.AddWithValue("@CompanyStructure", application.CompanyStructure);
                    command.Parameters.AddWithValue("@BusinessSince", application.BusinessSince);
                    command.Parameters.AddWithValue("@PhoneNumber", application.PhoneNumber);
                    command.Parameters.AddWithValue("@StreetAddress", application.StreetAddress);
                    command.Parameters.AddWithValue("@City", application.City);
                    command.Parameters.AddWithValue("@State", application.State);
                    command.Parameters.AddWithValue("@ZipCode", application.ZipCode);
                    command.Parameters.AddWithValue("@BankName", (object)application.BankName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AccountNumber", (object)application.AccountNumber ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IFSCCode", (object)application.IFSCCode ?? DBNull.Value);
                    command.Parameters.AddWithValue("@BankBranch", (object)application.BankBranch ?? DBNull.Value);
                    command.Parameters.AddWithValue("@BankState", (object)application.BankState ?? DBNull.Value);
                    command.Parameters.AddWithValue("@BankCity", (object)application.BankCity ?? DBNull.Value);
                    command.Parameters.AddWithValue("@BankZipCode", (object)application.BankZipCode ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LogoImagePath", (object)application.LogoImagePath ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            TempData["SuccessMessage"] = "Application submitted successfully!";
            return RedirectToAction("Index");
        }

        // --- THIS IS THE UPDATED METHOD ---
        [Authorize(Roles = "Employee")]
        public IActionResult EmployeeView()
        {
            var applications = new List<EmployeeApplicationViewModel>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var tempApplications = new List<CreditApplication>();
                // Step 1: Get all applications using your unchanged procedure
                using (SqlCommand command = new SqlCommand("CreditApplications_GetAll", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tempApplications.Add(new CreditApplication
                            {
                                ApplicationID = (int)reader["ApplicationID"],
                                UserID = (int)reader["UserID"],
                                CompanyStructure = reader["CompanyStructure"].ToString(),
                                Status = reader["Status"].ToString(),
                                SubmittedAt = (DateTime)reader["SubmittedAt"]
                            });
                        }
                    }
                }

                // Step 2: Loop through each application and get the user's details
                foreach (var appData in tempApplications)
                {
                    BankUser userDetails = null;
                    // Make a second database call for each application
                    using (SqlCommand userCommand = new SqlCommand("Bank_Users_GetByID", connection))
                    {
                        userCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        userCommand.Parameters.AddWithValue("@UserID", appData.UserID);
                        using (SqlDataReader userReader = userCommand.ExecuteReader())
                        {
                            if (userReader.Read())
                            {
                                userDetails = new BankUser
                                {
                                    FirstName = userReader["FirstName"].ToString(),
                                    LastName = userReader["LastName"].ToString(),
                                    Email = userReader["Email"].ToString()
                                };
                            }
                        }
                    }

                    // Combine the application data and user data into the final ViewModel
                    if (userDetails != null)
                    {
                        applications.Add(new EmployeeApplicationViewModel
                        {
                            ApplicationID = appData.ApplicationID,
                            ApplicantFullName = $"{userDetails.FirstName} {userDetails.LastName}",
                            ApplicantEmail = userDetails.Email,
                            CompanyStructure = appData.CompanyStructure,
                            Status = appData.Status,
                            SubmittedAt = appData.SubmittedAt
                        });
                    }
                }

                connection.Close();
            }

            return View(applications);
        }

        [HttpGet]
        public JsonResult GetCitiesByState(string state)
        {
            var locationService = new LocationService();
            var cities = locationService.GetCities(state);
            return Json(cities);
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Details(int id)
        {
            ApplicationDetailsViewModel details = null;
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreditApplications_GetDetailsByID", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicationID", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            details = new ApplicationDetailsViewModel
                            {
                                ApplicationID = (int)reader["ApplicationID"],
                                ApplicantFullName = $"{reader["FirstName"]} {reader["LastName"]}",
                                ApplicantEmail = reader["Email"].ToString(),
                                CompanyStructure = reader["CompanyStructure"].ToString(),
                                BusinessSince = (DateTime)reader["BusinessSince"],
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                StreetAddress = reader["StreetAddress"].ToString(),
                                City = reader["City"].ToString(),
                                State = reader["State"].ToString(),
                                ZipCode = reader["ZipCode"].ToString(),
                                BankName = reader["BankName"]?.ToString(),
                                AccountNumber = reader["AccountNumber"]?.ToString(),
                                IFSCCode = reader["IFSCCode"]?.ToString(),
                                BankBranch = reader["BankBranch"]?.ToString(),
                                BankState = reader["BankState"]?.ToString(),
                                BankCity = reader["BankCity"]?.ToString(),
                                BankZipCode = reader["BankZipCode"]?.ToString(),
                                LogoImagePath = reader["LogoImagePath"]?.ToString(),
                                Status = reader["Status"].ToString(),
                                SubmittedAt = (DateTime)reader["SubmittedAt"]
                            };
                        }
                    }
                }
                connection.Close();
            }

            if (details == null)
            {
                return NotFound();
            }

            return View(details);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public IActionResult UpdateStatus(int applicationId, string status)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreditApplications_UpdateStatus", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    command.Parameters.AddWithValue("@Status", status);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }

            return RedirectToAction("EmployeeView");
        }

        [Authorize(Roles = "User")]
        public IActionResult Edit(int id)
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            CreditApplication application = null;
            var locationService = new LocationService();
            ViewBag.States = locationService.GetStates();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreditApplications_GetByID", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicationID", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() && (int)reader["UserID"] == userId && reader["Status"].ToString() == "Pending")
                        {
                            application = new CreditApplication
                            {
                                ApplicationID = (int)reader["ApplicationID"],
                                UserID = (int)reader["UserID"],
                                CompanyStructure = reader["CompanyStructure"].ToString(),
                                BusinessSince = (DateTime)reader["BusinessSince"],
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                StreetAddress = reader["StreetAddress"].ToString(),
                                City = reader["City"].ToString(),
                                State = reader["State"].ToString(),
                                ZipCode = reader["ZipCode"].ToString(),
                                BankName = reader["BankName"]?.ToString(),
                                AccountNumber = reader["AccountNumber"]?.ToString(),
                                IFSCCode = reader["IFSCCode"]?.ToString(),
                                BankBranch = reader["BankBranch"]?.ToString(),
                                BankState = reader["BankState"]?.ToString(),
                                BankCity = reader["BankCity"]?.ToString(),
                                BankZipCode = reader["BankZipCode"]?.ToString(),
                                LogoImagePath = reader["LogoImagePath"]?.ToString(),
                                Status = reader["Status"].ToString(),
                                SubmittedAt = (DateTime)reader["SubmittedAt"]
                            };
                        }
                    }
                }
                connection.Close();
            }
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult Edit(CreditApplication application, IFormFile logoFile)
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!ModelState.IsValid)
            {
                var locationService = new LocationService();
                ViewBag.States = locationService.GetStates();
                return View(application);
            }
            if (logoFile != null && logoFile.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + logoFile.FileName;
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                Directory.CreateDirectory(uploadsFolder);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    logoFile.CopyTo(fileStream);
                }
                application.LogoImagePath = "/uploads/" + uniqueFileName;
            }
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreditApplications_Update", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicationID", application.ApplicationID);
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@CompanyStructure", application.CompanyStructure);
                    command.Parameters.AddWithValue("@BusinessSince", application.BusinessSince);
                    command.Parameters.AddWithValue("@PhoneNumber", application.PhoneNumber);
                    command.Parameters.AddWithValue("@StreetAddress", application.StreetAddress);
                    command.Parameters.AddWithValue("@City", application.City);
                    command.Parameters.AddWithValue("@State", application.State);
                    command.Parameters.AddWithValue("@ZipCode", application.ZipCode);
                    command.Parameters.AddWithValue("@BankName", (object)application.BankName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AccountNumber", (object)application.AccountNumber ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IFSCCode", (object)application.IFSCCode ?? DBNull.Value);
                    command.Parameters.AddWithValue("@BankBranch", (object)application.BankBranch ?? DBNull.Value);
                    command.Parameters.AddWithValue("@BankState", (object)application.BankState ?? DBNull.Value);
                    command.Parameters.AddWithValue("@BankCity", (object)application.BankCity ?? DBNull.Value);
                    command.Parameters.AddWithValue("@BankZipCode", (object)application.BankZipCode ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LogoImagePath", (object)application.LogoImagePath ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            TempData["SuccessMessage"] = "Application updated successfully!";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User")]
        public IActionResult Delete(int id)
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            CreditApplication application = null;
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreditApplications_GetByID", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicationID", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() && (int)reader["UserID"] == userId && reader["Status"].ToString() == "Pending")
                        {
                            application = new CreditApplication
                            {
                                ApplicationID = (int)reader["ApplicationID"],
                                CompanyStructure = reader["CompanyStructure"].ToString(),
                                SubmittedAt = (DateTime)reader["SubmittedAt"]
                            };
                        }
                    }
                }
                connection.Close();
            }
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "User")]
        public IActionResult DeleteConfirmed(int id)
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreditApplications_Delete", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicationID", id);
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            TempData["SuccessMessage"] = "Application deleted successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public IActionResult EmployeeDeleteConfirmed(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreditApplications_Delete", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicationID", id);
                    // Employee can delete any application, so no UserID parameter
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            TempData["SuccessMessage"] = "Application deleted successfully!";
            return RedirectToAction("EmployeeView");
        }

        [Authorize(Roles = "User")]
        public IActionResult Applications()
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var submittedApplications = new List<CreditApplication>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreditApplications_GetByUserID", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            submittedApplications.Add(new CreditApplication
                            {
                                ApplicationID = (int)reader["ApplicationID"],
                                UserID = (int)reader["UserID"],
                                CompanyStructure = reader["CompanyStructure"].ToString(),
                                BusinessSince = (DateTime)reader["BusinessSince"],
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                StreetAddress = reader["StreetAddress"].ToString(),
                                City = reader["City"].ToString(),
                                State = reader["State"].ToString(),
                                ZipCode = reader["ZipCode"].ToString(),
                                BankName = reader["BankName"]?.ToString(),
                                AccountNumber = reader["AccountNumber"]?.ToString(),
                                IFSCCode = reader["IFSCCode"]?.ToString(),
                                BankBranch = reader["BankBranch"]?.ToString(),
                                BankState = reader["BankState"]?.ToString(),
                                BankCity = reader["BankCity"]?.ToString(),
                                BankZipCode = reader["BankZipCode"]?.ToString(),
                                LogoImagePath = reader["LogoImagePath"]?.ToString(),
                                Status = reader["Status"].ToString(),
                                SubmittedAt = (DateTime)reader["SubmittedAt"]
                            });
                        }
                    }
                }
                connection.Close();
            }
            return View(submittedApplications);
        }
    }
}