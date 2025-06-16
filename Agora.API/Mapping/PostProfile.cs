using Agora.API.DTOs.Post;
using Agora.API.QueryParams;
using Agora.Core.Enums;
using Agora.Core.Models;
using Agora.Core.Models.Filters;
using Agora.Core.Models.Requests;
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
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Owner.UserName));
        
        CreateMap<Post, PostDetailsDto>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.PostCategoryName, opt => opt.MapFrom(src => src.PostCategory.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Owner.UserName));

        CreateMap<CreatePostDto, Post>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Trim()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Trim()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<PostType>(src.Type)))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        
        CreateMap<PostQueryParameters, PostFilter>();
        
        CreateMap<UpdatePostDetailsDto, PostDetailsUpdate>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title != null ? src.Title.Trim() : null))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description != null ? src.Description.Trim() : null))
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
    }
}