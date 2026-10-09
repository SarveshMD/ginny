using _01_TaskAPI.Models;

namespace _01_TaskAPI.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
