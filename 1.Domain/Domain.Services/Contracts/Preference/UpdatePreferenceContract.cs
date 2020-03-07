﻿using System;
using System.Collections.Generic;
using System.Text;
using Domain.Services.Contracts.User;

namespace Domain.Services.Contracts.Preference
{
    public class UpdatePreferenceContract
    {
        public int Id { get; set; }
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