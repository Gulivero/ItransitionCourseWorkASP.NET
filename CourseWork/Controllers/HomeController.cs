using CourseWork.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _db = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Collections = await _db.Collections
                .Include(c => c.Theme)
                .Include(u => u.User)
                .Include(e => e.Elements)
                .OrderByDescending(e => e.Elements.Count)
                .Take(3)
                .ToListAsync();
            ViewBag.LastAddedCollectionElement = await _db.CollectionElements
                .Include(t => t.Tags)
                .OrderByDescending(c => c.Id)
                .Take(3)
                .ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            return View();
        }
    }
}
