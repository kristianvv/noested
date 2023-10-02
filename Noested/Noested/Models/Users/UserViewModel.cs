using Noested.Entities;
using Noested.Repositories;

namespace Noested.Models.Users
{
    public class UserViewModel
    {
        public string Name { get; set; }
      
        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public List<UserEntity> Users { get; set; }
        
    }
}
