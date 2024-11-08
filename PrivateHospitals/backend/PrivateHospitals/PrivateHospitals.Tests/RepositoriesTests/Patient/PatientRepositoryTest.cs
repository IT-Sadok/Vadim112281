using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PrivateHospitals.Data.Data;
using PrivateHospitals.Data.Repositories.Patient;
using Xunit;

namespace PrivateHospitals.Tests.RepositoriesTests.Patient;

public class PatientRepositoryTest
{
    [Fact]
    public async Task AddPatient_ShouldAddPatientToDatabase_AndReturnTrue()
    {
        var options = new DbContextOptionsBuilder<HospitalDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        
        using var context = new HospitalDbContext(options);
        var repository = new PatientRepository(context);

        var patient = new Core.Models.Patient
        {
            Email = "test@gmail.com",
            FirstName = "TestName",
            LastName = "TestName",
            UserName = "TestUserName"
        };
        
        var result = await repository.AddPatientAsync(patient);

        result.Should().BeTrue();
        context.Patients.Should().ContainSingle(x => x.Email == "test@gmail.com" && x.FirstName == "TestName" && x.LastName == "TestName" && x.UserName == "TestUserName");
    }
    
}