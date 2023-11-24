using Microsoft.AspNetCore.Identity;
using Noested.Models;
using Noested.Data;
using Noested.Data.Repositories;

namespace Noested.Repositories;

public interface IUserRepository
{
    void Update(UserEntity user, List<string> roles);
    void Add(UserEntity user);
    List<UserEntity> GetUsers();
    void Delete(string email);
    bool IsAdmin(string email);
}

public class UserRepository : UserRepositoryBase, IUserRepository
{
    private readonly ApplicationDbContext dataContext;

    public UserRepository(ApplicationDbContext dataContext, UserManager<IdentityUser> userManager) : base(userManager)
    {
        this.dataContext = dataContext;
    }

    public void Delete(string email)
    {
        UserEntity? user = GetUserByEmail(email);
        if (user == null)
            return;
        dataContext.Users.Remove(user);
        dataContext.SaveChanges();
    }

    private UserEntity? GetUserByEmail(string email)
    {
        return dataContext.Users.FirstOrDefault(x => x.Email == email);
    }

    public List<UserEntity> GetUsers()
    {
        return dataContext.Users.ToList();
    }

    public void Add(UserEntity user)
    {
        var existingUser = GetUserByEmail(user.Email);
        if (existingUser != null)
        {
            throw new Exception("User already exists found");
        }
        dataContext.Users.Add(user);
        dataContext.SaveChanges();
    }
    public void Update(UserEntity user, List<string> roles)
    {
        var existingUser = GetUserByEmail(user.Email);
        if (existingUser == null)
        {
            throw new Exception("User not found");
        }

        existingUser.Email = user.Email;
        existingUser.Name = user.Name;
        dataContext.SaveChanges();
        SetRoles(user.Email, roles);
    }
}