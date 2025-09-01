namespace IceSync.Core.Services.Interfaces;

public interface IAuthenticationService
{
    Task<string> GetTokenAsync();
}