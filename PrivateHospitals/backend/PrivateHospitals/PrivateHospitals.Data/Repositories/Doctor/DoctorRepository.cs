using PrivateHospitals.Data.Data;
using PrivateHospitals.Data.Interfaces.Doctor;

namespace PrivateHospitals.Data.Repositories.Doctor;

public class DoctorRepository: IDoctorRepository
{
    private readonly HospitalDbContext _context;

    public DoctorRepository(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddDoctorAsync(Core.Models.Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();

        return true;
    }
}