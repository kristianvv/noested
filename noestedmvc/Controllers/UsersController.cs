using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Noested.Models.ViewModels;

namespace Noested.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users
                .Select(u => new UserViewModel

                {
                    UserId = u.Id,
                    Email = u.Email,
                    UserName = u.UserName,

                })
                .ToList();

            var viewModel = new UserListViewModel
            {
                Users = users
            };

            return View(viewModel);
        }


        public IActionResult Register()
        {
            return RedirectToPage("/Account/Register", new { area = "Identity" });
        }

        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                if (user != null)
                {
                    user.Email = model.Email;
                    user.PhoneNumber = user.PhoneNumber;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {

                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserViewModel
            {
                UserId = user.Id,
                UserName = user.UserName, // Optionally include the username in the confirmation view
                Email = user.Email,

            };

            return View(viewModel);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // If the deletion fails, return to the confirmation view with the user details
            var viewModel = new UserViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,// Optionally include the username in the confirmation view
                Email = user.Email,
            };

            return View("Delete", viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
            };

            return View(viewModel);
        }

    }

}