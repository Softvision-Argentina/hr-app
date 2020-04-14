using ApiServer.Contracts.Stage;
using AutoMapper;
using Domain.Services.Contracts.Stage;
using Domain.Services.Contracts.Stage.ClientStage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Profiles
{
    public class ClientStageProfile : Profile
    {
        public ClientStageProfile()
        {
            CreateMap<CreateClientStageViewModel, CreateClientStageContract>();
            CreateMap<CreatedClientStageContract, CreatedClientStageViewModel>();
            CreateMap<ReadedClientStageContract, ReadedClientStageViewModel>();
            CreateMap<UpdateClientStageViewModel, UpdateClientStageContract>();
        }
    }
}
