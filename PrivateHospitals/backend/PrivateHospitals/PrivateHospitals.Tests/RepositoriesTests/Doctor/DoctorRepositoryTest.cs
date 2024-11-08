using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PrivateHospitals.Data.Data;
using PrivateHospitals.Data.Repositories.Doctor;
using Xunit;

namespace PrivateHospitals.Tests.RepositoriesTests.Doctor;

public class DoctorRepositoryTest
{
    [Fact]
    public async Task AddDoctor_ShouldAddDocyotToDatabase_AndReturnTrue()
    {
        var options = new DbContextOptionsBuilder<HospitalDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new HospitalDbContext(options);
        var repository = new DoctorRepository(context);

        var doctor = new Core.Models.Doctor
        {
            FirstName = "TestName",
            LastName = "TestName",
            Email = "test@gmail.com",
            Speciality = "TestSpeciality",
            UserName = "TestUser",
        };
        
        var result = await repository.AddDoctorAsync(doctor);
        
        result.Should().BeTrue();   
        context.Doctors.Should().ContainSingle(x => x.FirstName == "TestName" && x.LastName == "TestName" && x.Email == "test@gmail.com" && x.Speciality == "TestSpeciality" && x.UserName == "TestUser");
    }
}