namespace _305.Tests.Integration.Base.JWT;

public interface IJwtProvider
{
    string GenerateToken(
        string userId = "1",
        string userName = "test-user",
        IEnumerable<string>? roles = null,
        IDictionary<string, string>? extraClaims = null);

    void AddTokenToClient(HttpClient client,
        string? userId = null,
        string? userName = null,
        IEnumerable<string>? roles = null,
        IDictionary<string, string>? extraClaims = null);
}
