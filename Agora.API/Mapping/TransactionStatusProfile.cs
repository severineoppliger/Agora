using Agora.API.DTOs.TransactionStatus;
using AutoMapper;
using TransactionStatus = Agora.Core.Models.TransactionStatus;

namespace Agora.API.Mapping;

public class TransactionStatusProfile : Profile
{
    public TransactionStatusProfile()
    {
        CreateMap<TransactionStatus, TransactionStatusSummaryDto>();
        
        CreateMap<TransactionStatus, TransactionStatusDetailsDto>()
            .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions));
        
        CreateMap<CreateTransactionStatusDto, TransactionStatus>();
        
        CreateMap<UpdateTransactionStatusDto, TransactionStatus>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}