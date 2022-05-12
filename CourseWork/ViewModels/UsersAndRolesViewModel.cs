using CourseWork.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CourseWork.ViewModels
{
    public class UsersAndRolesViewModel
    {
        public IEnumerable<User> Users { get; set; }

        public UserManager<User> userManager { get; set; }
    }
}
