using CourseWork.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _db;

        public ProfileController(UserManager<User> userManager, ApplicationContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            ViewBag.User = await _userManager.FindByIdAsync(id);
            ViewBag.UserCollections = await _db.Collections.Where(u => u.User.Id == id).ToListAsync();
            return View();
        }
    }
}
