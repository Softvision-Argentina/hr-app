using Domain.Model;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.Cv;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Interfaces.Repositories
{
    public interface ICvRepository
    {
        Cv GetCv(int id);
        bool SaveAll(Cv cv);
    }
}
