using PrivateHospitals.Application.Dtos.Patient;
using PrivateHospitals.Application.Responses;

namespace PrivateHospitals.Application.Interfaces.Patient;

public interface IPatientService
{
    Task<ServiceResponse<bool>> CreatePatientAsync(RegisterPatientDto patientDto);
}