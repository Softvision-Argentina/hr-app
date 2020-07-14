using ApiServer.Contracts.Community;
using ApiServer.Contracts.Office;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Contracts.OpenPosition
{
    public class ReadedOpenPositionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Seniority Seniority { get; set; }
        public string Studio { get; set; }
        public ReadedCommunityViewModel Community { get; set; }
        public bool Priority { get; set; }
        public string JobDescription { get; set; }
    }
}
