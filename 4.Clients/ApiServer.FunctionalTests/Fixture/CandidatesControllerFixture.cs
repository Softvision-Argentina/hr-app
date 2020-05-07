using System.Collections.Generic;
using System.Linq;
using ApiServer.Contracts.Candidates;
using ApiServer.Contracts.CandidateSkill;
using ApiServer.FunctionalTests.Core;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.FunctionalTests.Fixture
{
    public class CandidatesControllerFixture : BaseFunctionalTestFixture
    {
        public CandidatesControllerFixture()
        {
            ControllerName = "Candidates";
            //SeedCandidate();
        }
        public enum FilterType
        {
            Match, //will match an entity
            DontMatch //will not match any entity
        }

        public List<Candidate> GetCandidateList()
        {
            var netCommunity = new Community()
            {
                Name = "Net",
                Profile = new CandidateProfile() { Name = "candidate profile 1" }
            };

            var devopsCommunity = new Community()
            {
                Name = "Net",
                Profile = new CandidateProfile() { Name = "candidate profile 2" }
            };

            var almagroOffice = new Office()
            {
                Name = "Almagro"
            };

            var vicenteLopezOffice = new Office()
            {
                Name = "Vicente Lopez"
            };

            var netSkill = new Skill()
            {
                Name = "Entity Framework"
            };

            var devopsSkill = new Skill()
            {
                Name = "Jenkins"
            };

            var expectedCandidate = new Candidate()
            {
                Name = "this will meet search criteria",
                PreferredOffice = almagroOffice,
                Community = netCommunity,
                CandidateSkills = new List<CandidateSkill>()
                {
                    new CandidateSkill() {Skill = netSkill, Rate = 9}
                }
            };

            var candidateList = new List<Candidate>()
                {
                    new Candidate() //skill rate is low
                    {
                        Name = "This candidate will not meet search criteria",
                        PreferredOffice = almagroOffice,
                        Community = netCommunity,
                        CandidateSkills = new List<CandidateSkill>()
                        {
                            new CandidateSkill() {Skill = netSkill, Rate = 1}
                        }
                    },

                    new Candidate() //not the community asked
                    {
                        Name = "This candidate will not meet search criteria either",
                        PreferredOffice = vicenteLopezOffice,
                        Community = devopsCommunity,
                        Profile = new CandidateProfile(),
                        CandidateSkills = new List<CandidateSkill>()
                        {
                            new CandidateSkill() {Skill = devopsSkill}
                        }
                    },

                    expectedCandidate
                };

            return candidateList;
        }

        private FilterCandidateViewModel GetValidModel()
        {
            var validFilterCandidateSkillViewModel = new List<FilterCandidateSkillViewModel>()
            {
                new FilterCandidateSkillViewModel()
                {
                    SkillId = Context.
                        Skills.
                        AsNoTracking().
                        First(_ => _.Name == "Entity Framework").Id, MinRate = 5, MaxRate = 10
                }
            };

            var validModel = new FilterCandidateViewModel()
            {
                Community = Context.Community.First(_ => _.Name == "Net").Id,
                PreferredOffice = Context.Office.First(_ => _.Name == "Almagro").Id,
                SelectedSkills = validFilterCandidateSkillViewModel
            };

            return validModel;
        }

        private FilterCandidateViewModel GetInvalidModel()
        {
            var invalidFilterCandidateSkillViewModel = new List<FilterCandidateSkillViewModel>()
            {
                new FilterCandidateSkillViewModel() {SkillId = 999, MinRate = 999, MaxRate = 999}
            };

            var invalidModel = new FilterCandidateViewModel()
            {
                Community = 999,
                PreferredOffice = 999,
                SelectedSkills = invalidFilterCandidateSkillViewModel
            };

            return invalidModel;
        }

        public FilterCandidateViewModel GetFilterCandidateViewModel(FilterType filterType)
        {
            return filterType.Equals(FilterType.Match) ? GetValidModel() : GetInvalidModel();
        }

        public void Dispose()
        {
            Context.Dispose();
            Client.Dispose();
            Server.Dispose();
        }
    }


}
