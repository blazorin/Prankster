using System;
using System.Threading.Tasks;
using Shared.Dto;

namespace Model.Services
{
    public interface IProfileServices
    {
        Task<UserProfileDto> GetProfile(string id);
        /*
        Task<bool> UpdateBirth(string id, DateTime? newBirth);
        Task<bool> UpdateCountry(string id, string country);
        Task<bool> UpdateUsername(string id, string username);
        Task<bool> UpdateEmail(string id, string email);
        */
    }
}