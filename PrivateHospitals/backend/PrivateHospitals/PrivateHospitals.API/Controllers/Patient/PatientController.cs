using Microsoft.AspNetCore.Mvc;
using PrivateHospitals.Application.Dtos.Patient;
using PrivateHospitals.Application.Dtos.User;
using PrivateHospitals.Application.Interfaces.Patient;
using PrivateHospitals.Application.Interfaces.User;

namespace PrivateHospitals.API.Controllers.Patient;

[Route("api/patient")]
[ApiController]
public class PatientController: ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly IUserService _userService;

    public PatientController(IPatientService patientService, IUserService userService)
    {
        _patientService = patientService;
        _userService = userService;
    }

    [HttpPost("RegisterPatient")]
    public async Task<IActionResult> RegisterPatient([FromBody]RegisterPatientDto patientDto)
    {
        var patient =  await _patientService.CreatePatientAsync(patientDto);

        if (!patient.Success)
        {
            return BadRequest(patient.Errors);
        }

        return Ok(patient);
    }
}