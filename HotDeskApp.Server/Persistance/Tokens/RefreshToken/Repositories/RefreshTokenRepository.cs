using System.Diagnostics.CodeAnalysis;
using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Tokens.RefreshToken.Repositories;
using NHibernate;
using ISession = NHibernate.ISession;

namespace HotDeskApp.Server.Persistance.Tokens.RefreshToken.Repositories;

/// <summary>
///     Repository for managing refresh tokens.
/// </summary>
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ISession _session;
    private readonly IUnitOfWork _unitOfWork;
    
    public RefreshTokenRepository(ISession session, IUnitOfWork unitOfWork)
    {
        _session = session;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    ///     Generates a new refresh token for a specified user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The generated refresh token.</returns>
    public async Task<Models.Tokens.RefreshToken.Entities.RefreshToken> GenerateRefreshToken(Models.Tokens.RefreshToken.Entities.RefreshToken refreshToken)
    {
        _unitOfWork.BeginTransaction();
        try
        {
            await _session.SaveAsync(refreshToken);
            _unitOfWork.Commit();
            return refreshToken;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    /// <summary>
    ///     Gets a refresh token by its token string.
    /// </summary>
    /// <param name="token">The refresh token string.</param>
    /// <returns>The refresh token if found and not revoked; otherwise, null.</returns>
    public async Task<HotDeskApp.Server.Models.Tokens.RefreshToken.Entities.RefreshToken> GetRefreshToken(string token)
    {
        return await _session.GetAsync<Models.Tokens.RefreshToken.Entities.RefreshToken>(token);
        
    }
    
    
    /// <summary>
    ///     Revokes a refresh token.
    /// </summary>
    /// <param name="token">The refresh token string.</param>
    public async Task RevokeRefreshToken(string token)
    {
        _unitOfWork.BeginTransaction();
        try
        {
            var existingToken = await _session.QueryOver<HotDeskApp.Server.Models.Tokens.RefreshToken.Entities.RefreshToken>()
                .Where(rt => rt.Token == token)
                .SingleOrDefaultAsync();
            if (existingToken != null)
            {
                existingToken.IsRevoked = true;
                await _session.UpdateAsync(existingToken);
                _unitOfWork.Commit();
            }
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}