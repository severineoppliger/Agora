using Agora.API.DTOs.Account;
using Agora.API.DTOs.AppUser;
using Agora.Core.Models;
using AutoMapper;

namespace Agora.API.Mapping;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<RegisterDto, AppUser>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())  // Hashing is handled in controller
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore())
            .ForMember(dest => dest.Credit, opt => opt.Ignore());;

        CreateMap<SignInDto, AppUser>();

        CreateMap<AppUser, AccountDetailsDto>()
            .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts))
            .ForMember(dest => dest.TransactionsAsBuyer, opt => opt.MapFrom(src => src.TransactionsAsBuyer))
            .ForMember(dest => dest.TransactionsAsSeller, opt => opt.MapFrom(src => src.TransactionsAsSeller));

        CreateMap<AppUser, AccountSummaryDto>();
    }
}