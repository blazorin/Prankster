using AutoMapper;
using Model.Data;
using Shared.Dto;

namespace Model.Mapping
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// CreateMap
        /// </summary>
        public MappingProfile()
        {
            // User Mapping
            CreateMap<User, UserProfileDto>();
            CreateMap<User, UserDto>();
            CreateMap<User, AdminUserDto>();
            CreateMap<UserLoginDto, User>()
                .ForMember(u => u.Email, opt => opt.Ignore())
                .ForMember(u => u.IsBanned, opt => opt.Ignore())
                .ForMember(u => u.IsPremium, opt => opt.Ignore())
                .ForMember(u => u.TermsAccepted, opt => opt.Ignore())
                .ForMember(u => u.EndpointCreated, opt => opt.Ignore())
                .ForMember(u => u.EndpointUsername, opt => opt.Ignore())
                .ForMember(u => u.UserId, opt => opt.Ignore())
                .ForMember(u => u.Calls, opt => opt.Ignore())
                .ForMember(u => u.CallBalance, opt => opt.Ignore())
                .ForMember(u => u.Logs, opt => opt.Ignore())
                .ForMember(u => u.Perms, opt => opt.Ignore())
                .ForMember(u => u.Transactions, opt => opt.Ignore())
                .ForMember(u => u.Refers, opt => opt.Ignore())
                .ForMember(u => u.IsReferred, opt => opt.Ignore())
                .ForMember(u => u.LikedPranks, opt => opt.Ignore())
                .ForMember(u => u.DeviceModels, opt => opt.Ignore())
                .ForMember(u => u.IPAddresses, opt => opt.Ignore())
                .ForMember(u => u.IdentifierType, opt => opt.Ignore())
                ;


            CreateMap<Prank, PrankDto>();
            CreateMap<Package, PackageDto>();
            CreateMap<Transaction, AdminTransactionDto>();
            CreateMap<Call, CallDto>();
        }
    }
}