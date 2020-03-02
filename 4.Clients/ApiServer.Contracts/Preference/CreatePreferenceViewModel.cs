using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Contracts.Preference
{
    public class CreatePreferenceViewModel
    {
        public bool CasualtiesDashboard { get; set; }

        public bool CompletedDashboard { get; set; }

        public bool ProcessesDashboard { get; set; }

        public bool ProgressDashboard { get; set; }

        public bool ProjectionDashboard { get; set; }

        public bool SkillsDashboard { get; set; }

        public bool TimeToFill1Dashboard { get; set; }

        public bool TimeToFIll2Dashboard { get; set; }

        public int UserId { get; set; }
    }
}
