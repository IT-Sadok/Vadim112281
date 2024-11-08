using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using PrivateHospitals.Core.Models;

public class UserServiceTest
{
    private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    public UserServiceTest()
    {
        var userStoreMock = new Mock<IUserStore<AppUser>>();
        _userManagerMock = new Mock<UserManager<AppUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null
        );

        _signInManagerMock = new Mock<SignInManager<AppUser>>(
            _userManagerMock.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<AppUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<AppUser>>().Object
        );
    }

    [Fact]
    public async Task TestUserLoginAsync_ShouldReturnTrue_WhenLoginIsSuccessful()
    {
        var user = new AppUser { UserName = "testuser", Email = "testuser@example.com" };
        var password = "Password123!";

        _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName)).ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.PasswordSignInAsync(user.UserName, password, false, false))
                          .ReturnsAsync(SignInResult.Success);

        var result = await _signInManagerMock.Object.PasswordSignInAsync(user.UserName, password, false, false);

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task TestUserLoginAsync_ShouldReturnFalse_WhenLoginFails()
    {
        var user = new AppUser { UserName = "testuser", Email = "testuser@example.com" };
        var password = "IncorrectPassword";

        _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName)).ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.PasswordSignInAsync(user.UserName, password, false, false))
                          .ReturnsAsync(SignInResult.Failed);

        var result = await _signInManagerMock.Object.PasswordSignInAsync(user.UserName, password, false, false);

        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task TestUserLoginAsync_ShouldReturnFalse_WhenUserNotFound()
    {   
        var userName = "nonexistentuser";
        var password = "AnyPassword";

        _userManagerMock.Setup(x => x.FindByNameAsync(userName)).ReturnsAsync((AppUser)null);
        
        var user = await _userManagerMock.Object.FindByNameAsync(userName);
        
        user.Should().BeNull();
    }
    
    
}
