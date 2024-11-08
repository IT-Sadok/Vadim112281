using PrivateHospitals.Application.Dtos.Doctor;
using PrivateHospitals.Application.Responses;

namespace PrivateHospitals.Application.Interfaces.Doctor;

public interface IDoctorService
{
    Task<ServiceResponse<bool>> RegisterDoctorAsync(RegisterDoctorDto doctorDto);
}