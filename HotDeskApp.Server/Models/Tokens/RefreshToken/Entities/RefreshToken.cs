﻿namespace HotDeskApp.Server.Models.Tokens.RefreshToken.Entities;

public class RefreshToken
{
    public RefreshToken()
    {
    }

    public RefreshToken(Guid id, string token, Guid userId, DateTime expiration, bool isRevoked)
    {
        Id = id;
        Token = token;
        UserId = userId;
        Expiration = expiration;
        IsRevoked = isRevoked;
    }

    public virtual Guid Id { get; set; }
    public virtual string Token { get; set; }
    public virtual Guid UserId { get; set; }
    public virtual DateTime Expiration { get; set; }
    public virtual bool IsRevoked { get; set; }
}