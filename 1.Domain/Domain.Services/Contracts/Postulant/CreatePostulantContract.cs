using System;

namespace Domain.Services.Contracts.Postulant
{
    public class CreatePostulantContract
    {
        public string Name { get; set; }
        public string EmailAdress { get; set; }
        public string LinkedInProfile { get; set; }
        public string Cv { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
