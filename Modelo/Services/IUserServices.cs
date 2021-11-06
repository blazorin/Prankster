using System.Threading.Tasks;
using Model.Data;
using Model.Enums;
using Model.Utils;
using Shared;
using Shared.Dto;
using Shared.Enums;

namespace Model.Services
{
    public interface IUserServices
    {
        Task<User> GetUserByAuthenticationAsync(UserLoginDto credentials, UserLogType logType);
        Task<User> AddUserAsync(UserLoginDto newUserDto, UserLogType logType);
        Task<User> HandleOauthAuthenticationAsync(UserLoginDto credentials, OauthType oauthType, bool emailExists);
        Task<bool> IdentifierExistsAsync(string identifier);

        Task<bool> EmailExistsAsync(string email);

        Task<bool> IsBannedAsync(string identifier);

        Task<bool> HandleAuthCheckAsync(string userId = "", BasicUserLogDto basicUserLog);

        /*
        Task<bool> UsernameExistsAsync(string username);
        */
    }
}