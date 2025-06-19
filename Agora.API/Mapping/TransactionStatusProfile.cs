using Agora.API.DTOs.TransactionStatus;
using Agora.Core.Commands;
using AutoMapper;
using TransactionStatus = Agora.Core.Models.Entities.TransactionStatus;

namespace Agora.API.Mapping;

public class TransactionStatusProfile : Profile
{
    public TransactionStatusProfile()
    {
        CreateMap<TransactionStatus, TransactionStatusSummaryDto>();
        
        CreateMap<TransactionStatus, TransactionStatusDetailsDto>()
            .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions));
        
        CreateMap<UpdateTransactionStatusDetailsDto, UpdateTransactionStatusDetailsCommand>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name != null ? src.Name.Trim() : null))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description != null ? src.Description.Trim() : null))
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
    }
}