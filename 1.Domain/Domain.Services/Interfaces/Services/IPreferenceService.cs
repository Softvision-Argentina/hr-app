using System;
using System.Collections.Generic;
using System.Text;
using Domain.Model;
using Domain.Services.Contracts.Preference;

namespace Domain.Services.Interfaces.Services
{
    public interface IPreferenceService
    {
        IEnumerable<ReadedPreferenceContract> List();
        ReadedPreferenceContract Read(int id);
        void Update(UpdatePreferenceContract contract);
    }
}
