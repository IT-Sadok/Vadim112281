using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using PrivateHospitals.Core.Models;
using PrivateHospitals.Data.Repositories.User;
using Xunit;

namespace PrivateHospitals.Tests.RepositoriesTests.User;

public class UserRepositoryTest
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly UserRepository _userRepository;

    public UserRepositoryTest()
    {
        var store = new Mock<IUserStore<AppUser>>();
        _userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
        _userRepository = new UserRepository(_userManagerMock.Object);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        var email = "test@test.com";
        var expectedUser = new AppUser { Email = email };
        _userManagerMock.Setup(x => x.Users)
            .Returns((new List<AppUser> { expectedUser }).AsQueryable().BuildMockDbSet().Object);
        
        var user = await _userRepository.GetUserByEmailAsync(email);

        user.Should().NotBeNull();
        user.Email.Should().Be(email);  
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnSuccess_WhenUserCreated()
    {
        var user = new AppUser { Email = "test@test.com", UserName = "TestName", LastName = "TestLastName", FirstName = "TestFirstName"};
        var password = "TestPassword";
        _userManagerMock.Setup(x => x.CreateAsync(user, password))
            .ReturnsAsync(IdentityResult.Success);
        
        var result = await _userRepository.CreateUserAsync(user, password);
        
        result.Succeeded.Should().BeTrue();
    }
    
    [Fact]
    public async Task AddUserToRoleAsync_ShouldReturnSuccess_WhenRoleAdded()
    {
        var user = new AppUser { UserName = "testUser", Email = "test@example.com" };
        var roleName = "Patient";
        _userManagerMock.Setup(um => um.AddToRoleAsync(user, roleName))
            .ReturnsAsync(IdentityResult.Success);
        
        var result = await _userRepository.AddUserToRoleAsync(user, roleName);
        
        result.Succeeded.Should().BeTrue();
    }
}