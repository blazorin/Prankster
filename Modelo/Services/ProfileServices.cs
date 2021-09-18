using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Shared.Dto;
using Shared.Enums;

namespace Model.Services
{
    public class ProfileServices : IProfileServices
    {
        private readonly MauiPContext _ctx;
        private readonly IMapper _mapper;

        public ProfileServices(MauiPContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> GetProfile(string id)
        {
            var storedProfile = await _ctx.Users
                .Where(u => u.UserId == id)
                .ProjectTo<UserProfileDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return storedProfile;
        }
        /*

        public async Task<bool> UpdateBirth(string id, DateTime? newBirth)
        {
            var storedProfile = await FindCrowdUser(id);

            storedProfile.Birth = newBirth;
            storedProfile.Logs.Add(new UserLog
            {
                Date = DateTime.Now, UserLogId = Guid.NewGuid().ToString() + Guid.NewGuid(),
                UserLogType = UserLogType.BirthChanged
            });

            return await _ctx.SaveChangesAsync() != 1;
        }

        public async Task<bool> UpdateCountry(string id, string country)
        {
            var storedProfile = await FindCrowdUser(id);

            storedProfile.Country = country;
            storedProfile.Logs.Add(new UserLog
            {
                Date = DateTime.Now, UserLogId = Guid.NewGuid().ToString() + Guid.NewGuid(),
                UserLogType = UserLogType.CountryChanged
            });

            return await _ctx.SaveChangesAsync() != 1;
        }

        public async Task<bool> UpdateUsername(string id, string username)
        {
            var storedProfile = await FindCrowdUser(id);

            storedProfile.Name = username;
            storedProfile.Logs.Add(new UserLog
            {
                Date = DateTime.Now, UserLogId = Guid.NewGuid().ToString() + Guid.NewGuid(),
                UserLogType = UserLogType.UsernameChanged
            });

            return await _ctx.SaveChangesAsync() != 1;
        }

        public async Task<bool> UpdateEmail(string id, string email)
        {
            var storedProfile = await FindCrowdUser(id);

            storedProfile.Email = email;
            storedProfile.Logs.Add(new UserLog
            {
                Date = DateTime.Now, UserLogId = Guid.NewGuid().ToString() + Guid.NewGuid(),
                UserLogType = UserLogType.EmailChanged
            });

            return await _ctx.SaveChangesAsync() != 1;
        }
        */

        private async Task<User> FindCrowdUser(string id)
        {
            return await _ctx.Users
                .Where(u => u.UserId == id)
                .Include(u => u.Logs)
                .FirstOrDefaultAsync();
        }
    }
}