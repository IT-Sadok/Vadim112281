using Microsoft.AspNetCore.Mvc;
using PrivateHospitals.Application.Dtos.Doctor;
using PrivateHospitals.Application.Interfaces.Doctor;

namespace PrivateHospitals.API.Controllers.Doctor;

[Route("api/doctor")]
[ApiController]
public class DoctorController: ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpPost("RegisterDoctor")]
    public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorDto doctorDto)
    {
        var result = await _doctorService.RegisterDoctorAsync(doctorDto);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Ok(doctorDto);
    }
}