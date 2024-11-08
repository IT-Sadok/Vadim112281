namespace PrivateHospitals.Core.Models;

public class Patient
{
     public int PatientId { get; set; }
     public string FirstName { get; set; }
     public string LastName { get; set; }
     public string UserName { get; set; }
     public string Email { get; set; }
     public string? MedicalCard { get; set; }
 }