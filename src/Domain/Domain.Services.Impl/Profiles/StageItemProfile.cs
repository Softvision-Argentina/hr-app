using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Stage.StageItem;

namespace Domain.Services.Impl.Profiles
{
    public class StageItemProfile : Profile
    {
        public StageItemProfile()
        {
            CreateMap<CreateStageItemContract, StageItem>();
            CreateMap<StageItem, CreatedStageItemContract>();
            CreateMap<UpdateStageItemContract, StageItem>();
        }
    }
}
