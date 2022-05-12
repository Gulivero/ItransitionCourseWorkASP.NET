using CourseWork.Models;
using CourseWork.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
    public class CollectionController : Controller
    {
        private readonly ApplicationContext _db;

        public CollectionController(ApplicationContext db)
        {
            _db = db;
        }
        public async Task<List<Tag>> MakeTagList(string str)
        {
            var tags = new List<Tag>();
            if (!string.IsNullOrWhiteSpace(str))
            {
                foreach (var tag in str.Split())
                {
                    if (await _db.Tags.AnyAsync(t => t.Name == tag))
                    {
                        var existTag = await _db.Tags.FirstOrDefaultAsync(t => t.Name == tag);
                        tags.Add(existTag);
                    }
                    else
                    {
                        var newTag = new Tag
                        {
                            Name = tag
                        };
                        await _db.Tags.AddAsync(newTag);
                        await _db.SaveChangesAsync();
                        tags.Add(newTag);
                    }
                }
            }
            return tags;
        }
        [Authorize(Roles = "Unblocked")]
        public async Task<IActionResult> CreateCollection(string UserId)
        {
            ViewBag.Themes = await _db.Themes.ToListAsync();
            ViewBag.UserId = UserId;
            return View();
        }
        [Authorize(Roles = "Unblocked")]
        public async Task<IActionResult> CreateCollectionElement(int id)
        {
            ViewBag.Collection = await _db.Collections
                .Include(a => a.AdditionalFieldsNames)
                .Where(c => c.Id == id)
                .SingleAsync();
            return View();
        }
        public async Task<IActionResult> Collection(int id)
        {
            ViewBag.Collection = await _db.Collections
                .Include(u => u.User)
                .AsSingleQuery()
                .Include(t => t.Theme)
                .Where(c => c.Id == id)
                .SingleAsync();
            ViewBag.Elements = await _db.CollectionElements
                .Where(e => e.Collection.Id == id)
                .ToListAsync();

            return View();
        }
        public async Task<IActionResult> CollectionElement(int id)
        {
            ViewBag.CollectionElement = await _db.CollectionElements
                .Include(l => l.Likes)
                .ThenInclude(u => u.User)
                .Include(c => c.Comments)
                .ThenInclude(u => u.User)
                .Include(c => c.Collection)
                .ThenInclude(u => u.User)
                .Include(c => c.AdditionalFields)
                .ThenInclude(c => c.Name)
                .Include(t => t.Tags)
                .Where(c => c.Id == id)
                .SingleAsync();
            return View();
        }
        [Authorize(Roles = "Unblocked")]
        [HttpPost]
        public async Task<IActionResult> CreateCollectionElement(CreateCollectionElementViewModel model)
        {
            var collection = _db.Collections.FirstOrDefault(c => c.Id == model.CollectionId);
            foreach (var additionalField in model.AdditionalFields)
            {
                additionalField.Name =
                    _db.AdditionalFieldNames.FirstOrDefault(c => c.Id == additionalField.Name.Id);
            }

            if (ModelState.IsValid)
            {
                var collectionElement = new CollectionElement()
                {
                    Collection = collection,
                    Name = model.Name,
                    Tags = await MakeTagList(model.Tags),
                    AdditionalFields = model.AdditionalFields
                };
                await _db.AddAsync(collectionElement);
                await _db.SaveChangesAsync();
                return RedirectToAction("CollectionElement", "Collection", new { collectionElement.Id });
            }
            ViewBag.Collection = await _db.Collections
                .Include(a => a.AdditionalFieldsNames)
                .Where(c => c.Id == model.CollectionId)
                .SingleAsync();
            return View(model);
        }
        [Authorize(Roles = "Unblocked")]
        [HttpPost]
        public async Task<IActionResult> CreateCollection(CreateCollectionViewModel model)
        {
            var user = await _db.Users.FindAsync(model.UserId);
            var theme = await _db.Themes.FindAsync(model.Theme.Id);
            if (ModelState.IsValid)
            {
                Collection collection = new Collection()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Theme = theme,
                    User = user,
                    AdditionalFieldsNames = model.AdditionalFieldNames
                };


                await _db.Collections.AddAsync(collection);
                await _db.SaveChangesAsync();


                return RedirectToAction("Profile", "Profile", new { user.Id });
            }
            ViewBag.Themes = await _db.Themes.ToListAsync();
            ViewBag.UserId = user.Id;
            return View(model);
        }
        [Authorize(Roles = "Unblocked")]
        public async Task<IActionResult> EditCollection(int id)
        {
            ViewBag.Collection = await _db.Collections
                .FirstOrDefaultAsync(i => i.Id == id);

            return View();
        }
        [Authorize(Roles = "Unblocked")]
        [HttpPost]
        public async Task<IActionResult> EditCollection(EditCollectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var collection = await _db.Collections.FirstOrDefaultAsync(i => i.Id == model.Id);
                collection.Name = model.Name;
                collection.Description = model.Description;
                _db.Update(collection);
                await _db.SaveChangesAsync();
                return RedirectToAction("Collection", "Collection", new { id = model.Id });
            }

            return View(model);
        }
        [Authorize(Roles = "Unblocked")]
        public async Task<IActionResult> EditCollectionElement(int id)
        {
            var collectionElement = await _db.CollectionElements
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(i => i.Id == id);
            ViewBag.CollectionElement = collectionElement;
            var tagsList = collectionElement.Tags;
            var tags = string.Empty;

            foreach (var tag in tagsList)
            {
                tags += tag.Name + " ";
            }
            tags = tags.Remove(tags.Length - 1, 1);

            ViewBag.Tags = tags;

            return View();
        }
        [Authorize(Roles = "Unblocked")]
        [HttpPost]
        public async Task<IActionResult> EditCollectionElement(EditCollectionElementViewModel model)
        {
            var collectionElement = await _db.CollectionElements
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(i => i.Id == model.Id);
            if (ModelState.IsValid)
            {
                collectionElement.Name = model.Name;
                collectionElement.Tags = await MakeTagList(model.Tags);
                
                _db.Update(collectionElement);
                await _db.SaveChangesAsync();
                return RedirectToAction("CollectionElement", "Collection", new { id = model.Id });
            }
            ViewBag.CollectionElement = collectionElement;
            var tagsList = collectionElement.Tags;
            var tags = string.Empty;

            foreach (var tag in tagsList)
            {
                tags += tag.Name + " ";
            }
            tags = tags.Remove(tags.Length - 1, 1);

            ViewBag.Tags = tags;
            return View(model);
        }
        [Authorize(Roles = "Unblocked")]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            var collection = await _db.Collections
                .Include(x => x.AdditionalFieldsNames)
                .Include(e => e.Elements)
                .ThenInclude(c => c.Comments)
                .Include(e => e.Elements)
                .ThenInclude(l => l.Likes)
                .Include(e => e.Elements)
                .ThenInclude(t => t.Tags)
                .Include(e => e.Elements)
                .ThenInclude(a => a.AdditionalFields)
                .FirstOrDefaultAsync(x => x.Id == id);
            var user = _db.Collections.Include(x => x.User)
                                      .FirstOrDefault(x => x.Id == id).User;

            _db.Collections.Remove(collection);
            await _db.SaveChangesAsync();

            return RedirectToAction("Profile", "Profile", new { user.Id });
        }
        [Authorize(Roles = "Unblocked")]
        public async Task<IActionResult> DeleteCollectionElement(int elementId)
        {
            var collectionElement = await _db.CollectionElements
                .Include(x => x.AdditionalFields)
                .Include(e => e.Comments)
                .Include(l => l.Likes)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(x => x.Id == elementId);
            var id = _db.CollectionElements
                .Include(c => c.Collection)
                .FirstOrDefault(x => x.Id == elementId).Collection.Id;

            _db.CollectionElements.Remove(collectionElement);
            await _db.SaveChangesAsync();

            return RedirectToAction("Collection", "Collection", new { id });
        }
        [Authorize(Roles = "Unblocked")]
        [HttpPost]
        public async Task<IActionResult> CreateComment(CollectionElementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var collectionElement =
                    await _db.CollectionElements.FirstOrDefaultAsync(c => c.Id == model.CommentViewModel.CollectionElementId);
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == model.CommentViewModel.UserId);
                var comment = new Comment
                {
                    Text = model.CommentViewModel.Text,
                    User = user,
                    CollectionElement = collectionElement
                };
                await _db.AddAsync(comment);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("CollectionElement", "Collection", new { id = model.CommentViewModel.CollectionElementId });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CollectionElementLike(CollectionElementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var collectionElement =
                    await _db.CollectionElements.FirstOrDefaultAsync(c => c.Id == model.LikeViewModel.CollectionElementId);
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == model.LikeViewModel.UserId);
                var like = new Like
                {
                    User = user,
                    CollectionElement = collectionElement
                };
                await _db.AddAsync(like);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("CollectionElement", "Collection", new { id = model.LikeViewModel.CollectionElementId });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CollectionElementUnLike(CollectionElementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var like = await _db.Likes
                    .FirstOrDefaultAsync(l =>
                        l.CollectionElement.Id == model.LikeViewModel.CollectionElementId && l.User.Id == model.LikeViewModel.UserId);
                _db.Remove(like);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("CollectionElement", "Collection", new { id = model.LikeViewModel.CollectionElementId });
        }
    }
}
