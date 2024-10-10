using System.ComponentModel.DataAnnotations;
using HotDeskApp.Server.Models.UserEntity.Dtos;

namespace HotDeskApp.Server.Models.UserEntity.Services;

public interface IUserEntityService
{
    public Task<UserEntityDto?> GetUserInfo(Guid userId);
    public Task<UserEntityDto?> GetUserInfo(string userEmail);
    public Task<IEnumerable<UserEntityDto>> GetAllUsersInfo();
    public Task<Guid> CreateNewUser(Entities.UserEntity userEntity);
    public Task UpdateUser(Entities.UserEntity userEntity);
    public Task DeleteUser(Guid userId);
    public Task<Entities.UserEntity?> VerifyUser(UserLoginDto data);
    public Task<Entities.UserEntity> RegisterUser(UserRegisterDto data);
    public Task ValidateRegisterData(UserRegisterDto user);


}