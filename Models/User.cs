using System;
namespace Noested.Models
{
    public class User
    {
        public int Id { get; set; }
        public int EmployeeNumber { get; set; }
        public required string Password { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
    }

    public enum UserRole
    {
        Service,
        Mechanic,
        Administrator
    }
}

