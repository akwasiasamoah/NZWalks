using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class LoginRequestProfile : Profile
    {
        public LoginRequestProfile()
        {
            CreateMap<Models.Domain.User, Models.DTO.LoginRequest>().ReverseMap();
        }
    }
}
