using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.Enums;
using Model.Utils;
using Shared;
using Shared.Dto;
using Shared.Enums;
using Shared.Utils;

namespace Model.Services
{
    public class UserServices : IUserServices
    {
        private readonly MauiPContext _ctx;
        private readonly IMapper _mapper;

        public UserServices(MauiPContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<User> GetUserByAuthenticationAsync(UserLoginDto credentials, UserLogType logType)
        {
            User storedUser;

            if (logType is UserLogType.GoogleLogin or UserLogType.FacebookLogin)
            {
                storedUser = await _ctx.Users
                    .Where(user => user.Email == credentials.Email)
                    .Include(user => user.Logs)
                    .Include(user => user.DeviceModels)
                    .Include(user => user.IPAddresses)
                    .Include(user => user.Perms)
                    .FirstOrDefaultAsync();
            }
            else
            {
                //var passwordHashed = HashingHelper.ComputeSha256Hash(credentials.Password);

                storedUser = await _ctx.Users
                    .Where(user =>
                        user.Identifier == credentials.Identifier && user.Pin == credentials.Pin)
                    .Include(user => user.Logs)
                    .Include(user => user.DeviceModels)
                    .Include(user => user.IPAddresses)
                    .Include(user => user.Perms)
                    .FirstOrDefaultAsync();
            }

            if (storedUser == null)
                return null;

            // first Google (Gapi) or Facebook login with this email with the Identifier
            // if there was a previous email, access from it will be revoked
            if (logType is UserLogType.GoogleLinked or UserLogType.FacebookLinked)
                storedUser.Email = credentials.Email;

            storedUser.DeviceModels.Add(new Device
            {
                DeviceModel = string.IsNullOrEmpty(credentials.DeviceModel) ? "Unknown" : credentials.DeviceModel,
                DeviceId = Guid.NewGuid() + Guid.NewGuid().ToString()
            });
            storedUser.IPAddresses.Add(new IpAddress
            {
                Value = credentials.IPAddress, IpAddressId = Guid.NewGuid() + Guid.NewGuid().ToString()
            });

            // IdentifierType identification is not updated anymore since IdentifierType and Indentifier is only set at register

            storedUser.LastLogin = DateTime.Now;
            storedUser.LastPlatform = credentials.LastPlatform;
            storedUser.Logs.Add(new UserLog
                { Date = DateTime.Now, UserLogId = Guid.NewGuid().ToString() + Guid.NewGuid(), UserLogType = logType });


            await _ctx.SaveChangesAsync();

            return storedUser;
        }

        public async Task<User> AddUserAsync(UserLoginDto newUserDto, UserLogType logType)
        {
            var user = _mapper.Map<User>(newUserDto);

            user.Logs = new List<UserLog>
            {
                new()
                {
                    Date = DateTime.Now, UserLogType = logType, UserLogId = Guid.NewGuid().ToString() + Guid.NewGuid()
                }
            };

            user.Perms = new List<UserPerms>
            {
                new()
                {
                    PermKey = "vipType", PermValue = "newbie", UserPermsId = Guid.NewGuid().ToString()
                }
            };

            user.DeviceModels = new List<Device>
            {
                new()
                {
                    DeviceModel = string.IsNullOrEmpty(newUserDto.DeviceModel) ? "Unknown" : newUserDto.DeviceModel,
                    DeviceId = Guid.NewGuid() + Guid.NewGuid().ToString()
                }
            };

            user.IPAddresses = new List<IpAddress>
            {
                new()
                {
                    Value = newUserDto.IPAddress, IpAddressId = Guid.NewGuid() + Guid.NewGuid().ToString()
                }
            };

            user.UserId = Guid.NewGuid().ToString();
            user.CreationDate = DateTime.Now;
            user.LastLogin = DateTime.Now;
            user.IdentifierType = IdentifierType.KeychainIdentifier; //GetIdentifierType(newUserDto.Identifier);
            user.Pin = new Random().Next(1000, 9999);

            /*
            if (logType == UserLogType.SignUp)
                user.PasswordHashed = HashingHelper.ComputeSha256Hash(newUserDto.Password);
                */

            await _ctx.Users.AddAsync(user);
            await _ctx.SaveChangesAsync();

            return user;
        }

        public async Task<User> HandleOauthAuthenticationAsync(UserLoginDto oauthCredentials,
            OauthType oauthType, bool emailExists)
        {

            var loginLogType = oauthType switch
            {
                OauthType.Google => emailExists ? UserLogType.GoogleLogin : UserLogType.GoogleLinked,
                OauthType.Facebook => emailExists ? UserLogType.FacebookLogin : UserLogType.FacebookLinked,
                _ => throw new ArgumentOutOfRangeException(nameof(oauthType), oauthType, null)
            };

            return await GetUserByAuthenticationAsync(oauthCredentials, loginLogType);
        }

        public async Task<bool> IdentifierExistsAsync(string identifier)
        {
            return await _ctx.Users.AnyAsync(u => u.Identifier == identifier);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _ctx.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsBannedAsync(string identifier)
        {
            return await _ctx.Users.AnyAsync(u => u.IsBanned && u.Identifier == identifier);
        }

        private static IdentifierType GetIdentifierType(string identifier) =>
            IsPhoneNumber(identifier) ? IdentifierType.PhoneNumber : IdentifierType.SIM;


        private static bool IsPhoneNumber(string number) =>
            Regex.Match(number, @"^(\+[0-9]{9})$").Success;


        /*
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _ctx.Users.AnyAsync(u => u.Name == username);
        }
        */
    }
}