using Agora.API.DTOs.Post;
using Agora.Core.Commands;
using Agora.Core.Enums;
using Agora.Core.Models.Entities;
using AutoMapper;
using PostQueryParameters = Agora.Core.Models.DomainQueryParameters.PostQueryParameters;

namespace Agora.API.Mapping;

/// <summary>
/// AutoMapper profile that defines mappings between the <see cref="Post"/> domain model,
/// API DTOs, and command objects used for creating, updating, and querying posts.
/// </summary>
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
        
        CreateMap<ApiQueryParameters.PostQueryParameters, PostQueryParameters>();
        
        CreateMap<UpdatePostDetailsDto, UpdatePostDetailsCommand>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title != null ? src.Title.Trim() : null))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description != null ? src.Description.Trim() : null))
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
    }
}