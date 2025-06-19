using Agora.API.DTOs.User;
using Agora.Core.Commands;
using Agora.Core.Models.Entities;
using AutoMapper;

namespace Agora.API.Mapping;

/// <summary>
/// AutoMapper profile that defines mappings between the <see cref="User"/> domain model,
/// API DTOs, and command objects used for user registration, authentication, and data projection.
/// </summary>
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserDto, RegisterUserCommand>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName.Trim()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()));

        CreateMap<SignInUserDto, SignInUserCommand>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()));
        
        CreateMap<User, UserDetailsDto>()
            .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts))
            .ForMember(dest => dest.TransactionsAsBuyer, opt => opt.MapFrom(src => src.TransactionsAsBuyer))
            .ForMember(dest => dest.TransactionsAsSeller, opt => opt.MapFrom(src => src.TransactionsAsSeller));

        CreateMap<User, UserSummaryDto>();
        CreateMap<User, UserEmailDto>();
    }
}