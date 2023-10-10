using CleanSample.Application.DTOs;
using CleanSample.Domain.Aggregates;

namespace CleanSample.Application.Mappings;

internal class CustomerMapper: Profile
{
    public CustomerMapper()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.FirstName.Value))
            .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.LastName.Value))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.Value))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone.Value))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.BankAccountNumber, opt => opt.MapFrom(src => src.BankAccount.Number));
    }
}