using Agora.API.DTOs.User;
using Agora.Core.Enums;
using Agora.Core.Models;
using AutoMapper;

namespace Agora.API.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserSummaryDto>();

        CreateMap<User, UserDetailsDto>()
            .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts))
            .ForMember(dest => dest.TransactionsAsBuyer, opt => opt.MapFrom(src => src.TransactionsAsBuyer))
            .ForMember(dest => dest.TransactionsAsSeller, opt => opt.MapFrom(src => src.TransactionsAsSeller));
        
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())  // Hashing is handled in controller
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore())
            .ForMember(dest => dest.Credit, opt => opt.Ignore());
    }
    
}