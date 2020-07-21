using Domain.Model.Enum;
using Domain.Services.Contracts.Community;
using Domain.Services.Contracts.Office;

namespace Domain.Services.Contracts.OpenPositions
{
    public class CreateOpenPositionContract
    {
        public string Title { get; set; }
        public Seniority Seniority { get; set; }
        public string Studio { get; set; }
        public ReadedCommunityContract Community { get; set; }
        public bool Priority { get; set; }
        public string JobDescription { get; set; }
    }
}
