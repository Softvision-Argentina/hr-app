using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Impl.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, ReadedUserContract>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community));
            CreateMap<CreateUserContract, User>();
            CreateMap<User, CreatedUserContract>();
            CreateMap<UpdateUserContract, User>();
        }
    }
}
