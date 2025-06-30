using Agora.API.DTOs.PostCategory;
using Agora.Core.Commands;
using Agora.Core.Models.Entities;
using AutoMapper;

namespace Agora.API.Mapping;

/// <summary>
/// AutoMapper profile that defines mappings between <see cref="PostCategory"/> domain model
/// and its corresponding DTOs used for creation and data projection.
/// </summary>
public class PostCategoryProfile : Profile
{
    public PostCategoryProfile()
    {
        CreateMap<PostCategory, PostCategorySummaryDto>();
        
        CreateMap<PostCategory, PostCategoryDetailsDto>()
            .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts));
        
        CreateMap<CreatePostCategoryDto, PostCategory>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()));
        
        CreateMap<UpdatePostCategoryDetailsDto, UpdatePostCategoryDetailsCommand>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
    }
}