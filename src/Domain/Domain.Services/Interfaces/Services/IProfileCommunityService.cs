namespace Domain.Services.Interfaces.Services
{
    using Domain.Model;
    using Domain.Services.Contracts.OpenPositions;
    using Domain.Services.Contracts.ProfileByCommunity;
    using System;
    using System.Collections.Generic;

    public interface IProfileCommunityService
    {
        CreatedProfileCommunityContract Create(CreateProfileCommunityContract profileByCommunityContract);

        IEnumerable<ReadedProfileCommunityContract> Get(int id);

        void Update(UpdateProfileCommunityContract openPosition);

        void Delete(int id);
    }
}
