using AutoMapper;
using CartonCaps.Application.DTOs;
using CartonCaps.Domain.Entities;

namespace CartonCaps.Application.Mappings
{
    public class CartonCapsProfile : Profile
    {
        public CartonCapsProfile() {

            CreateMap<UserReferralEntity, UserReferralResponse>();
            CreateMap<ReferralVisitHistoryEntity, ReferralVisitHistoryResponse>();
        }
    }
}
