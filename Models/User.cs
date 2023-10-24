using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace LoginMVC.Models


{
    public class User
    {

        public int IdUser { get; set; }
        [Required(ErrorMessage = "Please enter the FirstName ")]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        /*public string? MiddleName { get; set; }*/
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Please enter the LastName ")]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter the Email ")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter the DateOfBirth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }


        [Required(ErrorMessage = "Please enter the PhoneNumber ")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits long and contain only numeric characters.")]
        [StringLength(10, ErrorMessage = "Phone number must be 10 digits long.")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Please enter the Nationality ")]
        //[Table("CountryMaster ")]
        //public string Nationality { get; set; }
        public int Nationality { get; set; }

        public IEnumerable<SelectListItem> Nationalities { get; set; }

        [Required(ErrorMessage = "Please enter the UserName ")]
        //[Remote("IsUserNameUnique", "Home", HttpMethod = "POST", ErrorMessage = "This username is already in use.")]
        public string UserName { get; set; }

        //public bool IsUserNameUnique { get; set; }
        
        [Required(ErrorMessage = "Please enter the Password ")]
        
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]

        public string Password { get; set; }
        [Required(ErrorMessage = "Please ConfirmPassword ")]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
        //public DataTable UserData { get; set; }
       /* public User(DataTable dataTable)
        {
            // Populate properties from the DataTable here
            // For example:
            FirstName = dataTable.Rows[0]["FirstName"].ToString();
            MiddleName = dataTable.Rows[0]["MiddleName"].ToString();
            LastName = dataTable.Rows[0]["LastName"].ToString();
            Email = dataTable.Rows[0]["Email"].ToString();
            // Initialize other properties as needed
        }*/
    }
}