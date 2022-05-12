using CourseWork.Models;
using CourseWork.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> AdminPanel()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return View("Error", new string[] { "Ваш аккаунт был удалён" });
            }
            if (await _userManager.IsInRoleAsync(user, "Blocked"))
            {
                return View("Error", new string[] { "Ваш аккаунт был заблокирован" });
            }
            user.LastLoginDate = DateTime.Now;
            IdentityResult result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return View(new UsersAndRolesViewModel { Users = _userManager.Users, userManager = _userManager });
            }
            else
            {
                return View("Error", result.Errors);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(List<string> usersId, string action)
        {
            if (usersId == null || usersId.Count == 0)
            {
                return RedirectToAction("AdminPanel");
            }
            IdentityResult result;
            foreach (string id in usersId)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    if (action == "Delete")
                    {

                        result = await _userManager.DeleteAsync(user);
                        if (!result.Succeeded)
                        {
                            return View("Error", result.Errors);
                        }

                    }
                    else if (action == "Unblock")
                    {
                        if (await _userManager.IsInRoleAsync(user, "Blocked"))
                        {
                            result = await _userManager.RemoveFromRoleAsync(user, "Blocked");
                            if (!result.Succeeded)
                            {
                                return View("Error", result.Errors);
                            }

                            result = await _userManager.AddToRoleAsync(user, "Unblocked");

                            if (!result.Succeeded)
                            {
                                return View("Error", result.Errors);
                            }
                        }
                    }
                    else if (action == "Block")
                    {
                        if (await _userManager.IsInRoleAsync(user, "Unblocked"))
                        {
                            result = await _userManager.RemoveFromRoleAsync(user, "Unblocked");
                            if (!result.Succeeded)
                            {
                                return View("Error", result.Errors);
                            }

                            result = await _userManager.AddToRoleAsync(user, "Blocked");

                            if (!result.Succeeded)
                            {
                                return View("Error", result.Errors);
                            }
                        }
                    }
                    else if (action == "Admin")
                    {
                        if (!await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            result = await _userManager.AddToRoleAsync(user, "Admin");

                            if (!result.Succeeded)
                            {
                                return View("Error", result.Errors);
                            }
                        }
                    }
                    else if (action == "User")
                    {
                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            result = await _userManager.RemoveFromRoleAsync(user, "Admin");

                            if (!result.Succeeded)
                            {
                                return View("Error", result.Errors);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("AdminPanel");
        }
    }
}
