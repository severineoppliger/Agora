using Agora.API.DTOs.PostCategory;
using Agora.Core.Models;
using AutoMapper;

namespace Agora.API.Mapping;

public class PostCategoryProfile : Profile
{
    public PostCategoryProfile()
    {
        CreateMap<PostCategory, PostCategorySummaryDto>();
        
        CreateMap<PostCategory, PostCategoryDetailsDto>()
            .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts));
        
        CreateMap<CreatePostCategoryDto, PostCategory>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()));
    }
}