using Agora.API.DTOs.Post;
using Agora.Core.Enums;
using Agora.Core.Models;
using AutoMapper;

namespace Agora.API.Mapping;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostSummaryDto>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.PostCategoryName, opt => opt.MapFrom(src => src.PostCategory.Name))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
        
        CreateMap<Post, PostDetailsDto>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.PostCategoryName, opt => opt.MapFrom(src => src.PostCategory.Name))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
        
        CreateMap<CreatePostDto, Post>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<PostType>(src.Type)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<PostStatus>(src.Status)))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        
        CreateMap<UpdatePostDto, Post>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<PostType>(src.Type)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<PostStatus>(src.Status)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}