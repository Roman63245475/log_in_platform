namespace be.Authorization;

public interface IAuthorizationService
{
    public string GenerateJwtToken(User user);
}