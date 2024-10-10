namespace HotDeskApp.Server.Models.Tokens.BlacklistToken.Entities;

public class BlacklistToken
{
    public BlacklistToken()
    {
    }

    public BlacklistToken(string token, Guid userId, DateTime endOfBlacklisting)
    {
        Token = token;
        UserId = userId;
        EndOfBlacklisting = endOfBlacklisting;
    }

    public virtual string Token { get; set; }
    public virtual Guid UserId { get; set; }
    public virtual DateTime EndOfBlacklisting { get; set; }
}