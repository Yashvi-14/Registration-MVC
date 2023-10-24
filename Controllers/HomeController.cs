using System;
using System.Configuration;
using System.Web.Mvc;
using LoginMVC.Models;
using LoginMVC.Helpers;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
namespace LoginMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult About()
        {
            return View();
        }

        public ActionResult SignUpSuccess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            try
            {
                string query = "SELECT * FROM UserMaster_Yashvi WHERE UserName = @UserName AND Password = @Password";
                var mySqlParams = new List<SqlParameter>
                {
                    new SqlParameter("@UserName", login.UserName),
                    new SqlParameter("@Password", login.Password)
                };

                DataTable result = DBHelper.GetDataTable(query, mySqlParams);

                if (result != null && result.Rows.Count > 0)
                {
                    int userId = Convert.ToInt32(result.Rows[0]["IdUser"]);
                    Session["UserId"] = userId;
                    Session["UserName"] = result.Rows[0]["UserName"].ToString();

                    return RedirectToAction("About");
                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid username or password.");
                    return View("Login");
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Password", "An error occurred while logging in.");
                return View("Login");
            }
        }

        private void SetNationalities()
        {
             var model = new User();
            string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionStr"].ConnectionString;
            string query = "SELECT IdCountry, CountryName FROM CountryMaster";

            List<SelectListItem> nationalities = new List<SelectListItem>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nationalities.Add(new SelectListItem
                                {
                                    Value = reader["IdCountry"].ToString(),
                                    Text = reader["CountryName"].ToString()
                                });
                            }
                        }
                    }
                }


                ViewBag.Nationalities = new SelectList(nationalities, "Value", "Text");

                model.Nationalities = ViewBag.Nationalities;
        }
            catch (Exception ex)
            {
                // Handle exceptions here (e.g., log the error)
                ModelState.AddModelError("", "An error occurred while loading data for signup.");
                //return View(model);
            }
        }
    
        [HttpGet]
        public ActionResult SignUp()
        {
            SetNationalities();
                return View();
            }
         
        

        [HttpPost]
        public ActionResult SignUp(User user)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    string qry = "SELECT UserName FROM UserMaster_Yashvi WHERE UserName = @UserName";
                    var myParams = new List<SqlParameter>
                {
                    new SqlParameter("@UserName", user.UserName),
                };
                    DataTable res = DBHelper.GetDataTable(qry, myParams);

                    // Selectingg countryy 
                    /*  List<SelectListItem> nationalities = new List<SelectListItem>();
                      ViewBag.Nationalities = new SelectList(nationalities, "Value", "Text");*/
                    SetNationalities();

                    string currentUser = User.Identity.Name;
                    DateTime currentTimestamp = DateTime.Now;
                    if (res != null && res.Rows.Count > 0)
                    {
                        ModelState.AddModelError("UserName", "Username is already in use. Please try another one.");
                        return View(user);
                    }
                   
                    string query = "INSERT INTO UserMaster_Yashvi (FirstName, MiddleName, LastName, Email, DateOfBirth, PhoneNumber, Nationality, UserName, Password, CreatedBy, CreatedOn)" +
                        " VALUES (@FirstName, @MiddleName, @LastName, @Email, @DateOfBirth, @PhoneNumber, @Nationality, @UserName, @Password, @CreatedBy, @CreatedOn)";

                    var mySqlParams = new List<SqlParameter>
                {
                    new SqlParameter("@FirstName", user.FirstName),
                    new SqlParameter("@MiddleName", string.IsNullOrEmpty(user.MiddleName) ? DBNull.Value : (object)user.MiddleName),
                    new SqlParameter("@LastName", user.LastName),
                    new SqlParameter("@Email", user.Email),
                    new SqlParameter("@DateOfBirth", user.DateOfBirth),
                    new SqlParameter("@PhoneNumber", user.PhoneNumber),
                    new SqlParameter("@Nationality", user.Nationality),
                    new SqlParameter("@UserName", user.UserName),
                    new SqlParameter("@Password", user.Password),
                    new SqlParameter("@CreatedBy", currentUser),
                    new SqlParameter("@CreatedOn", currentTimestamp)
                };



                    int result = DBHelper.ExecuteQuery(query, mySqlParams);

                    //ViewBag.Nationalities = new SelectList(nationalities, "Value", "Text");

                    //return RedirectToAction("SignUpSuccess");
                    return RedirectToAction("UserDetails");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("An error occurred while signing up.", ex);
                    return View(user);
                }
            }
            else
            {
                return View();
            }

        }
        [HttpGet]
        public ActionResult UserDetails()
        {
            try
            {
                
                string query = "SELECT * FROM UserMaster_Yashvi";
                var mySqlParams = new List<SqlParameter>();

                DataTable result = DBHelper.GetDataTable(query, mySqlParams);

                
                List<User> users = new List<User>();
                foreach (DataRow row in result.Rows)
                {
                    User user = new User
                    {
                        FirstName = row["FirstName"].ToString(),
                        MiddleName = row["MiddleName"].ToString(),
                        LastName = row["LastName"].ToString(),
                        Email = row["Email"].ToString(),
                        DateOfBirth = (DateTime)row["DateOfBirth"],
                        PhoneNumber = row["PhoneNumber"].ToString(),
                        Nationality = (int)row["Nationality"],
                        UserName = row["UserName"].ToString(),
                        Password = row["Password"].ToString(),
                        CreatedBy = row["CreatedBy"].ToString(),
                        CreatedOn = (DateTime)row["CreatedOn"],

                    };
                    users.Add(user);
                }

                return View(users);
            }
            catch (Exception ex)
            {
               
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View("Error");
            }
        }       
    }
}
