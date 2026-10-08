namespace _01_TaskAPI.DTOs;

public record RegisterRequestDto(string Email, string Name, string Password);

public record LoginRequestDto(string Email, string Password);

public record AuthResponseDto(Guid Id, string Email, string Name, string Token);
