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
                .Include(c => c.Comments)
                .Where(e => (e.Name + e.Collection.Description + e.Collection.Theme.Name + e.Collection.User.UserName).Contains(model.Query) || e.Comments.Any(c => c.Text.Contains(model.Query)) || e.Tags.Any(t => t.Name.Contains(model.Query)))
                .ToListAsync();
            return View(model);
        }
    }
}
