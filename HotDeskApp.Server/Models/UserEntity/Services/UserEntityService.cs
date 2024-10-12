using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using HotDeskApp.Server.Models.UserEntity.Dtos;
using HotDeskApp.Server.Models.UserEntity.Enums;
using HotDeskApp.Server.Models.UserEntity.Repositories;

namespace HotDeskApp.Server.Models.UserEntity.Services;

public class UserEntityService : IUserEntityService
{
    private readonly IUserEntityRepository _userEntityRepository;

    public UserEntityService(IUserEntityRepository userEntityRepository)
    {
        _userEntityRepository = userEntityRepository;
    }

    //PASSWORDHASH RELATED FUNCTIONS
    private const int SaltSize = 16; // 128 bit
    private const int KeySize = 32; // 256 bit
    private const int Iterations = 10000;
    private const char Delimiter = ';';

    /// <summary>
    ///     Hashes a password using a secure hashing algorithm.
    /// </summary>
    /// <param name="password">The plain text password.</param>
    /// <returns>The hashed password.</returns>
    private static string HashingPassword(string password)
    {
        using (var algorithm = new Rfc2898DeriveBytes(
                   password,
                   SaltSize,
                   Iterations,
                   HashAlgorithmName.SHA256))
        {
            var salt = algorithm.Salt;
            var key = algorithm.GetBytes(KeySize);
            var base64Salt = Convert.ToBase64String(salt);
            var base64Key = Convert.ToBase64String(key);

            return $"{base64Salt}{Delimiter}{base64Key}";
        }
    }

    /// <summary>
    ///     Verifies a plain text password against a hashed password.
    /// </summary>
    /// <param name="password">The plain text password.</param>
    /// <param name="hashedPassword">The hashed password.</param>
    /// <returns><c>true</c> if the passwords match; otherwise, <c>false</c>.</returns>
    private static bool VerifyPassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split(Delimiter);
        if (parts.Length != 2) return false;

        var base64Salt = parts[0];
        var base64Key = parts[1];

        var salt = Convert.FromBase64String(base64Salt);
        var key = Convert.FromBase64String(base64Key);

        using (var algorithm = new Rfc2898DeriveBytes(
                   password,
                   salt,
                   Iterations,
                   HashAlgorithmName.SHA256))
        {
            var keyToCheck = algorithm.GetBytes(KeySize);
            return AreKeysEqual(key, keyToCheck);
        }
    }

    /// <summary>
    ///     Compares two byte arrays for equality.
    /// </summary>
    /// <param name="key1">The first byte array.</param>
    /// <param name="key2">The second byte array.</param>
    /// <returns><c>true</c> if the byte arrays are equal; otherwise, <c>false</c>.</returns>
    private static bool AreKeysEqual(byte[] key1, byte[] key2)
    {
        if (key1.Length != key2.Length) return false;

        for (var i = 0; i < key1.Length; i++)
            if (key1[i] != key2[i])
                return false;

        return true;
    }

    public async Task<Entities.UserEntity> RegisterUser(UserRegisterDto data)
    {
        var user = new Entities.UserEntity()
        {
            Name = data.Name,
            Surname = data.Surname,
            Email = data.Email,
            PasswordHash = HashingPassword(data.Password),
            UserRole = UserRole.User,
            LastTimeLogged = DateTime.Now
        };
        await _userEntityRepository.Save(user);
        return user;
    }

    /// <summary>
    ///     Verifies user by user's login credentials.
    /// </summary>
    /// <param name="data">The login credentials.</param>
    /// <returns>The user entity if verification is successful; otherwise, <c>null</c>.</returns>
    public async Task<Entities.UserEntity?> VerifyUser(UserLoginDto data)
    {
        var user = await _userEntityRepository.GetByEmailAsync(data.Email);
        if (user == null)
        {
            return null;
        }

        var verify = VerifyPassword(data.Password, user.PasswordHash);
        if (!verify)
        {
            return null;
        }

        user.LastTimeLogged = DateTime.UtcNow;
        await _userEntityRepository.Update(user);
        return user;
    }

    /// <summary>
    ///     Verifies user by user's login credentials.
    /// </summary>
    /// <param name="data">The login credentials.</param>
    /// <returns>The user entity if verification is successful; otherwise, <c>null</c>.</returns>
    public async Task<Entities.UserEntity?> VerifyUser(UserEntityDto userDto)
    {
        var user = await _userEntityRepository.GetByEmailAsync(userDto.Email);
        if (user == null)
        {
            return null;
        }

        user.LastTimeLogged = DateTime.UtcNow;
        await _userEntityRepository.Update(user);
        return user;
    }

    public async Task ValidateRegisterData(UserRegisterDto user)
    {
        if (user.Name.Length < 2 || user.Surname.Length < 2)
        {
            throw new ValidationException("Name or Surname is too short.");
        }

        var emailRegex = new Regex("^[^\\s@]+@[^\\s@]+\\.[^\\s@]{2,}$");
        if (!emailRegex.IsMatch(user.Email))
        {
            throw new ValidationException("Invalid email format.");
        }

        var passwordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$");
        if (!passwordRegex.IsMatch(user.Password))
        {
            throw new ValidationException(
                "Password must contain at least 8 characters, including 1 uppercase letter and 1 digit.");
        }
    }

    //BASIC SERVICE FUNCTIONS

    public async Task<UserEntityDto?> GetUserInfo(string userEmail)
    {
        var user = await _userEntityRepository.GetByEmailAsync(userEmail);
        return user != null ? user.ToDto() : null;
    }

    public async Task<UserEntityDto?> GetUserInfo(Guid userId)
    {
        var user = await _userEntityRepository.Get(userId);
        return user != null ? user.ToDto() : null;
    }

    public async Task<IEnumerable<UserEntityDto>> GetAllUsersInfo()
    {
        var list = await _userEntityRepository.GetAll();
        var newList = new List<UserEntityDto>();
        foreach (var user in list)
        {
            newList.Add(user.ToDto());
        }

        return newList;
    }

    public async Task<Guid> CreateNewUser(Entities.UserEntity userEntity)
    {
        userEntity.PasswordHash = HashingPassword(userEntity.PasswordHash);
        userEntity.LastTimeLogged = DateTime.Now;
        return await _userEntityRepository.Save(userEntity);
    }

    public async Task UpdateUser(Entities.UserEntity userEntity)
    {
        await _userEntityRepository.Update(userEntity);
    }

    public async Task DeleteUser(Guid userId)
    {
        await _userEntityRepository.Delete(userId);
    }
}