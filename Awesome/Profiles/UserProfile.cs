using AutoMapper;
using Awesome.DTOs.User;
using Awesome.Models.Entities;

namespace Awesome.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseDto>().ForMember(
                dest => dest.Email, opt => opt.MapFrom(src => src.Email)
            ).ForMember(
                dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)
            ).ForMember(
                dest => dest.FullName, opt => opt.MapFrom(src => src.FullName)
            )
            .ForMember(
                dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar)
            )
            .ForMember(
                dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth)
            )
            .ForMember(
                dest => dest.EmailVerified, opt => opt.MapFrom(src => src.EmailVerifiedAt != null)
            ).ForMember(
                dest => dest.PhoneNumberVerified, opt => opt.MapFrom(src => src.PhoneNumberVerifiedAt != null)
            ).ForMember(
                dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt)
            ).ForMember(
                dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt)
            );
    }
}