using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Domain.Model.Enum;

namespace Domain.Model
{
    public class Postulant: Entity<int>
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string LinkedInProfile { get; set; }
        public string Cv { get; set; }
        
    }
}
