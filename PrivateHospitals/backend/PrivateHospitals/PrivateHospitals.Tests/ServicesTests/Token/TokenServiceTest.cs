using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Moq;
using PrivateHospitals.Application.Services;
using PrivateHospitals.Core.Models;
using Xunit;
using FluentAssertions;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;
    private readonly Mock<IConfiguration> _configurationMock;

    public TokenServiceTests()
    {
        // Налаштування Mock<IConfiguration> з довгим ключем для JWT
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(x => x["JWT:SigningKey"]).Returns("super_secret_key_with_minimum_64_characters_long_1234567890_example_abcdefghij");
        _configurationMock.Setup(x => x["JWT:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(x => x["JWT:Audience"]).Returns("TestAudience");

        _tokenService = new TokenService(_configurationMock.Object);
    }

    [Fact]
    public void CreateToken_ShouldReturnValidJwtToken()
    {
        var user = new AppUser
        {
            Email = "test@test.com",
            UserName = "TestUser"
        };
        
        var token = _tokenService.CreateToken(user);
        
        token.Should().NotBeNullOrEmpty("токен не повинен бути порожнім");
        
        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.CanReadToken(token).Should().BeTrue("токен має бути валідним JWT");
        
        var jwtToken = tokenHandler.ReadJwtToken(token);
        
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.GivenName && c.Value == user.UserName);
        
        jwtToken.Issuer.Should().Be("TestIssuer");
        jwtToken.Audiences.Should().Contain("TestAudience");
        
        jwtToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), TimeSpan.FromMinutes(1), 
            "термін дії токену має бути 7 днів");
    }
}
