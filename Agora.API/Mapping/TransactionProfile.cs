using Agora.API.DTOs.Transaction;
using Agora.API.DTOs.TransactionStatus;
using Agora.Core.Models;
using AutoMapper;

namespace Agora.API.Mapping;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transaction, TransactionSummaryDto>()
            .ForMember(dest => dest.Post, opt=> opt.MapFrom(src => src.Post))
            .ForMember(dest => dest.TransactionStatusName, opt=> opt.MapFrom(src => src.TransactionStatus.Name))
            .ForMember(dest => dest.BuyerUsername, opt=> opt.MapFrom(src => src.Buyer.Username))
            .ForMember(dest => dest.SellerUsername, opt=> opt.MapFrom(src => src.Seller.Username));
        CreateMap<Transaction, TransactionDetailsDto>();
        CreateMap<CreateTransactionDto, Transaction>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        CreateMap<UpdateTransactionDto, Transaction>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}