using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Cv : Entity<int>
    {
        public string Url { get; set; }

        public Candidate Candidate { get; set; }

        public int CandidateId { get; set; }

        public string PublicId { get; set; }
    }
}
