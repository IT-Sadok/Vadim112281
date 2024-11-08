using Microsoft.AspNetCore.Mvc;
using PrivateHospitals.Application.Interfaces.User;
using LoginDto = PrivateHospitals.Application.Dtos.User.LoginDto;
using RegisterDto = PrivateHospitals.Application.Dtos.User.RegisterDto;

namespace PrivateHospitals.API.Controllers.User;

[Route("api/user")]
[ApiController]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("LoginUser")]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
    {
       var result = await _userService.LoginUserAsync(loginDto);

       if (!result.Success)
       {
           return BadRequest(result.Errors);
       }
       
       return Ok(result);
    }
}