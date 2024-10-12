using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Tokens.BlacklistToken.Repositories;
using ISession = NHibernate.ISession;

namespace HotDeskApp.Server.Persistance.Tokens.BlacklistToken.Repositories;

/// <summary>
///     Repository for managing token blacklisting.
/// </summary>
public class BlacklistTokenRepository : IBlacklistTokenRepository
{
    private readonly ISession _session;
    private readonly IUnitOfWork _unitOfWork;

    public BlacklistTokenRepository(ISession session, IUnitOfWork unitOfWork)
    {
        _session = session;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    ///     Adds a token to the blacklist.
    /// </summary>
    /// <param name="token">The token to be blacklisted.</param>
    /// <param name="userId">The ID of the user associated with the token.</param>
    /// <param name="timeOfRemoval">The time when the token should be removed from the blacklist.</param>
    public async void AddToBlacklist(Models.Tokens.BlacklistToken.Entities.BlacklistToken refreshToken)
    {
        _unitOfWork.BeginTransaction();
        try
        {
            await _session.SaveAsync(refreshToken);
            _unitOfWork.Commit();
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    /// <summary>
    ///     Removes a token from the blacklist.
    /// </summary>
    /// <param name="token">The token to be removed from the blacklist.</param>
    public async void RemoveFromBlacklist(string token)
    {
        _unitOfWork.BeginTransaction();
        try
        {
            var existingToken = await _session.GetAsync<Models.Tokens.BlacklistToken.Entities.BlacklistToken>(token);
            if (existingToken != null)
            {
                await _session.DeleteAsync(existingToken);
            }

            _unitOfWork.Commit();
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    /// <summary>
    ///     Checks if a token is blacklisted.
    /// </summary>
    /// <param name="token">The token to check.</param>
    /// <returns><c>true</c> if the token is blacklisted; otherwise, <c>false</c>.</returns>
    public async Task<bool> IsTokenBlacklisted(string token)
    {
        var existingToken = await _session.GetAsync<Models.Tokens.BlacklistToken.Entities.BlacklistToken>(token);
        return existingToken != null && (existingToken.EndOfBlacklisting > DateTime.Now);
    }
}