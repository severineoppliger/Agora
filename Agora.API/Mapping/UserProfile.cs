using Agora.API.DTOs.User;
using Agora.Core.Models;
using Agora.Core.Models.Requests;
using AutoMapper;

namespace Agora.API.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterDto, UserRegistrationInfo>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName.Trim()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()));

        CreateMap<SignInDto, UserSignInInfo>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()));
        
        CreateMap<User, UserDetailsDto>()
            .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts))
            .ForMember(dest => dest.TransactionsAsBuyer, opt => opt.MapFrom(src => src.TransactionsAsBuyer))
            .ForMember(dest => dest.TransactionsAsSeller, opt => opt.MapFrom(src => src.TransactionsAsSeller));

        CreateMap<User, UserSummaryDto>();
        CreateMap<User, UserEmailDto>();
    }
}