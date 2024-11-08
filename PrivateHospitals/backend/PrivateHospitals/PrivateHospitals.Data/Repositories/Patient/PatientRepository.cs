using Microsoft.EntityFrameworkCore;
using PrivateHospitals.Data.Data;
using PrivateHospitals.Data.Interfaces.Patient;

namespace PrivateHospitals.Data.Repositories.Patient;

public class PatientRepository: IPatientRepository
{
    private readonly HospitalDbContext _context;

    public PatientRepository(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddPatientAsync(Core.Models.Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        return true;
    }
    
}