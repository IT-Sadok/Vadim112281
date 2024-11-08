namespace PrivateHospitals.Application.Dtos.Doctor;

public class RegisterDoctorDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Speciality { get; set; }
    public string Password { get; set; }
}