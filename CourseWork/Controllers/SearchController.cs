using CourseWork.Models;
using CourseWork.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationContext _db;

        public SearchController(ApplicationContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchViewModel model)
        {
            var comments = await _db.Comments
                .Where(t => t.Text.Contains(model.Query))
                .ToListAsync();
            var unitedComments = string.Empty;

            foreach (var comment in comments)
            {
                unitedComments += comment.Text;
            }

            ViewBag.CollectionElements = await _db.CollectionElements
                .Include(c => c.Collection)
                .ThenInclude(u => u.User)
                .Include(t => t.Tags)
                .Where(c => (c.Name + c.Collection.Description + c.Collection.Theme.Name + c.Collection.User.UserName + unitedComments).Contains(model.Query))
                .ToListAsync();
            return View(model);
        }
    }
}
