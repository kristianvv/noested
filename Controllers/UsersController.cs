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
                .Select(u => new UserViewModel { UserId = u.Id, Email = u.Email })
                .ToList();

            var viewModel = new UserListViewModel
            {
                Users = users
            };

            return View(viewModel);
        }
    }
}
