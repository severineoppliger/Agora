﻿using Agora.API.ApiQueryParameters;
using Agora.API.DTOs.Transaction;
using Agora.Core.Commands;
using Agora.Core.Models.Entities;
using AutoMapper;

namespace Agora.API.Mapping;

/// <summary>
/// AutoMapper profile that defines mappings between the <see cref="Transaction"/> domain model,
/// API DTOs, and command objects used for creating, updating, and projecting transactions.
/// </summary>
public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transaction, TransactionSummaryDto>()
            .ForMember(dest => dest.PostTitle, opt => opt.MapFrom(src => src.Post!.Title))
            .ForMember(dest => dest.TransactionStatusName, opt => opt.MapFrom(src => src.TransactionStatus!.Name))
            .ForMember(dest => dest.BuyerUsername, opt => opt.MapFrom(src => src.Buyer!.UserName))
            .ForMember(dest => dest.SellerUsername, opt => opt.MapFrom(src => src.Seller!.UserName));
        
        CreateMap<Transaction, TransactionDetailsDto>()
            .ForMember(dest => dest.TransactionStatusName, opt => opt.MapFrom(src => src.TransactionStatus!.Name))
            .ForMember(dest => dest.InitiatorUsername, opt => opt.MapFrom(src => src.Initiator!.UserName))
            .ForMember(dest => dest.BuyerUsername, opt => opt.MapFrom(src => src.Buyer!.UserName))
            .ForMember(dest => dest.SellerUsername, opt => opt.MapFrom(src => src.Seller!.UserName))
            .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src =>
                src.TransactionDate.HasValue ? src.TransactionDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null));

        CreateMap<CreateTransactionDto, Transaction>()
            .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src =>
                src.TransactionDate.HasValue ? DateOnly.FromDateTime(src.TransactionDate.Value) : (DateOnly?)null))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        
        CreateMap<TransactionQueryParameters, Core.Models.DomainQueryParameters.TransactionQueryParameters>();

        CreateMap<UpdateTransactionDetailsDto, UpdateTransactionDetailsCommand>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title != null ? src.Title.Trim() : null))
            .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => 
                src.TransactionDate.HasValue ? DateOnly.FromDateTime(src.TransactionDate.Value) : (DateOnly?)null))
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
    }
}