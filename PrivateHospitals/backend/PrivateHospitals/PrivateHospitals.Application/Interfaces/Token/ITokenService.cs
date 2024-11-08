using PrivateHospitals.Core.Models;

namespace PrivateHospitals.Application.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}