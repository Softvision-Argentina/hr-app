using Domain.Model.Enum;
using Domain.Services.Contracts.Community;
using Domain.Services.Contracts.Office;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Contracts.OpenPositions
{
    public class UpdateOpenPositionContract
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Seniority Seniority { get; set; }
        public string Studio { get; set; }
        public ReadedCommunityContract Community { get; set; }
        public bool Priority { get; set; }
        public string JobDescription { get; set; }
    }
}
