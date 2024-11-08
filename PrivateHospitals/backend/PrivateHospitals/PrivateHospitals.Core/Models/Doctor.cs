namespace PrivateHospitals.Core.Models;

public class Doctor
{
    public int DoctorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Speciality { get; set; }
}