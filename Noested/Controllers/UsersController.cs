using Noested.Entities;
using Noested.Models.Users;
using Noested.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Noested.Controllers
{
    //[Authorize]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        [HttpGet]
        public IActionResult Index(string? email)
        {
            var model = new UserViewModel();
            model.Users = userRepository.GetUsers();
            if (email != null)
            {
                var currentUser = model.Users.FirstOrDefault(x => x.Email == email);
                if (currentUser != null)
                {
                    model.Email = currentUser.Email;
                    model.Name = currentUser.Name;
                    model.IsAdmin = userRepository.IsAdmin(currentUser.Email);

                    // Set the IsAdmin property based on user's admin status
                    model.IsAdmin = userRepository.IsAdmin(currentUser.Email);
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(UserViewModel model)
        {
            UserEntity newUser = new UserEntity
            {
                Name = model.Name,
                Email = model.Email,
                IsAdmin = model.IsAdmin
            };

            var roles = new List<string>();
            if (model.IsAdmin)
            {
                roles.Add("Administrator");
            }

            var existingUser = userRepository.GetUsers().FirstOrDefault(x => x.Email.Equals(newUser.Email, StringComparison.InvariantCultureIgnoreCase));

            if (existingUser != null)
            {
                existingUser.Name = newUser.Name;
                existingUser.IsAdmin = model.IsAdmin;
                userRepository.Update(existingUser, roles);
            }
            else
            {
                userRepository.Add(newUser);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(string email)
        {
            userRepository.Delete(email);
            return RedirectToAction("Index");
        }
    }
}
