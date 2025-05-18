using AutoMapper;
using InvoiceSystem.Application.DTO;
using InvoiceSystem.Application.Features.Invoices.Commands;
using InvoiceSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Application.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateInvoiceCommand, Invoice>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Draft"))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
         CreateMap<Invoice, CreateInvoiceCommand>();

        CreateMap<InvoiceItemDto, InvoiceItem>();
        CreateMap<InvoiceItem, InvoiceItemDto>();
    }
    public override string ProfileName => nameof(MappingProfile);
}