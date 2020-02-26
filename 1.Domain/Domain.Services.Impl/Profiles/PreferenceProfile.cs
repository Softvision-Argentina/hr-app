using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Preference;

namespace Domain.Services.Impl.Profiles
{
    public class PreferenceProfile : Profile
    {
        public PreferenceProfile()
        {
            //CreateMap<Preference, ReadedPreferenceContract>();
            //CreateMap<CreateRoomContract, Room>();
            //CreateMap<Room, CreatedRoomContract>();
            CreateMap<UpdatePreferenceContract, Preference>();
        }
    }
}
