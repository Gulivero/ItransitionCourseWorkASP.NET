using Microsoft.AspNetCore.Identity;
using System;

namespace CourseWork.Models
{
    public class User : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string Image { get; set; }
    }
}