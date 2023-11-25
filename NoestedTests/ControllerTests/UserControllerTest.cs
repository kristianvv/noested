using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Noested.Controllers;
using Noested.Models.ViewModels;

namespace NoestedTests.ControllerTests
{
    public class UsersControllerTest
    {
        [Fact]
        public async Task Index_ReturnsViewWithUserList()
        {
            // Arrange
            var userManagerMock = MockUserManager();
            var roleManagerMock = MockRoleManager();

            var controller = new UsersController(userManagerMock.Object, roleManagerMock.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<UserListViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model.Users);
            Assert.Equal(2, model.Users.Count()); // Adjust based on your expected user count
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenUserIdIsNull()
        {
            // Arrange
            var userManagerMock = MockUserManager();
            var roleManagerMock = MockRoleManager();

            var controller = new UsersController(userManagerMock.Object, roleManagerMock.Object);

            // Act
            var result = await controller.Edit(userId: null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Add more test methods for other actions as needed

        private static Mock<UserManager<IdentityUser>> MockUserManager()
        {
            var users = new List<IdentityUser>
            {
                new IdentityUser { Id = "1", UserName = "user1", Email = "user1@example.com" },
                new IdentityUser { Id = "2", UserName = "user2", Email = "user2@example.com" }
            }.AsQueryable();

            var store = new Mock<IUserStore<IdentityUser>>();
            store.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync((string userId, System.Threading.CancellationToken cancellationToken) =>
                {
                    return users.FirstOrDefault(u => u.Id == userId);
                });

            var userManagerMock = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.Users).Returns(users);

            return userManagerMock;
        }

        private static Mock<RoleManager<IdentityRole>> MockRoleManager()
        {
            var roles = new List<IdentityRole>
            {
                new IdentityRole { Id = "1", Name = "Admin" },
                new IdentityRole { Id = "2", Name = "User" }
            }.AsQueryable();

            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object, null, null, null, null);

            roleManagerMock.Setup(x => x.Roles).Returns(roles);

            return roleManagerMock;
        }
    }
}
