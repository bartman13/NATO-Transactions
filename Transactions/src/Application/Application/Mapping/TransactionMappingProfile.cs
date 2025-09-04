using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<Transaction, TransactionDto>();
        CreateMap<CreateTransactionDto, Transaction>();
    }
}
