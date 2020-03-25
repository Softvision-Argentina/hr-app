using Core;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Model.Seed;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System;
using System.Collections.Generic;

namespace Domain.Services.Repositories.EF
{
    public class SeedMigrator : Migrator<DataBaseContext>
    {
        public SeedMigrator(DataBaseContext context) : base(context)
        {
        }

        protected override void SeedData(DataBaseContext context)
        {
            SeedDummy(context);
        }

        private static void SeedDummy(DataBaseContext context)
        {
            #region Dummy
            var dummy1 = new Dummy { Name = "dummy1", Description = "Dummy 1", TestValue = "value of dummy 1" };
            var dummy2 = new Dummy { Name = "dummy2", Description = "Dummy 2", TestValue = "value of dummy 2" };
            var dummy3 = new Dummy { Name = "dummy3", Description = "Dummy 3", TestValue = "value of dummy 3" };
            context.Dummies.Add(dummy1);
            context.Dummies.Add(dummy2);
            context.Dummies.Add(dummy3);
            context.SaveChanges();
            #endregion

            #region CandidateProfiles
            var Profile1 = new CandidateProfile { Name = "Devs", Description = "Developers" };
            var Profile2 = new CandidateProfile { Name = "RRHH", Description = "Recursos Humanos" };
            var Profile3 = new CandidateProfile { Name = "MKT", Description = "Marketing" };
            context.Profiles.Add(Profile1);
            context.Profiles.Add(Profile2);
            context.Profiles.Add(Profile3);
            context.SaveChanges();
            #endregion

            #region Community
            var Community1 = new Community { Name = ".NET", Description = "Comunidad de .Net", ProfileId = 1, Profile = Profile1 };
            var Community2 = new Community { Name = "Devs", Description = "Comunidad de Developers", ProfileId = 1, Profile = Profile1 };
            var Community3 = new Community { Name = "RRHH", Description = "Comunidad de Recursos Humanos", ProfileId = 2, Profile = Profile2 };
            var Community4 = new Community { Name = "Marketing", Description = "Comunidad de Marketing", ProfileId = 3, Profile = Profile3 };
            context.Community.Add(Community1);
            context.Community.Add(Community2);
            context.Community.Add(Community3);
            context.Community.Add(Community4);
            context.SaveChanges();
            #endregion

            #region Consultants
            var consultant1 = new Consultant { Name = "Cristian", LastName = "Rodríguez", PhoneNumber = "(+54)12312312", EmailAddress = "critian.rodriguez@softvision.com" };
            var consultant2 = new Consultant { Name = "Santiago", LastName = "Mayochi", PhoneNumber = "(+1)2123453423", EmailAddress = "santiago.mayochi@softvision.com", AdditionalInformation = "HR Analyst" };
            var consultant3 = new Consultant { Name = "Samanta", LastName = "Fioretti", PhoneNumber = "(+1)2123453423", EmailAddress = "samanta.fioretti@softvision.com" };
            var consultant4 = new Consultant { Name = "Fernanda", LastName = "García Utrera", PhoneNumber = "(+1)2123453423", EmailAddress = "fernanda.garciautrera@softvision.com" };
            var consultant5 = new Consultant { Name = "Matías", LastName = "Baldi", PhoneNumber = "(+1)2123453423", EmailAddress = "matias.baldi@softvision.com", AdditionalInformation = "Community Manager Argentina" };
            var consultant6 = new Consultant { Name = "Giselle", LastName = "Chanis", PhoneNumber = "(+54)2123453423", EmailAddress = "giselle.chanis@softvision.com" };
            var consultant7 = new Consultant { Name = "Ignacio", LastName = "Costantini", PhoneNumber = "(+54)2123453423", EmailAddress = "ignacio.costantini@softvision.com" };
            var consultant8 = new Consultant { Name = "Cristian", LastName = "Piqué", PhoneNumber = "(+54)2355677581", EmailAddress = "cristian.pique@softvision.com" };
            var consultant9 = new Consultant { Name = "Lucas", LastName = "Stirpe", PhoneNumber = "(+54)12359742", EmailAddress = "lucas.stirpe@softvision.com" };
            var consultant10 = new Consultant { Name = "Damian", LastName = "Fernandez Urroz", PhoneNumber = "(+54)129876543", EmailAddress = "damian.fernandezurroz@softvision.com" };
            var consultant11 = new Consultant { Name = "Adrian", LastName = "Rodriguez Renda", PhoneNumber = "(+54)9876543", EmailAddress = "adrian.rodriguezrenda@softvision.com" };
            var consultant12 = new Consultant { Name = "Kevin", LastName = "Zatel", PhoneNumber = "(+54)12359742", EmailAddress = "kevin.zatel@softvision.com" };
            var consultant13 = new Consultant { Name = "Tomas", LastName = "Rebollo", PhoneNumber = "(+54)12359742", EmailAddress = "tomas.rebollo@softvision.com" };
            var consultant14 = new Consultant { Name = "Facundo", LastName = "Valeriano", PhoneNumber = "(+54)12359742", EmailAddress = "facundo.valeriano@softvision.com" };
            var consultant15 = new Consultant { Name = "Matias", LastName = "Totaro", PhoneNumber = "(+54)12359742", EmailAddress = "matias.totaro@softvision.com" };
            var consultant19 = new Consultant { Name = "Teo", LastName = "Benavides", PhoneNumber = "(+54)12359742", EmailAddress = "teo.benavides@softvision.com" };
            var consultant20 = new Consultant { Name = "Tomas", LastName = "Guagnini", PhoneNumber = "(+54)12359742", EmailAddress = "tomas.guagnini@softvision.com" };
            var consultant23 = new Consultant { Name = "Tomas", LastName = "Reisenman", PhoneNumber = "(+54)49794582", EmailAddress = "tomas.reisenman@softvision.com" };
            var consultant21 = new Consultant { Name = "Nicolas", LastName = "Roldan", PhoneNumber = "(+54)49794582", EmailAddress = "nicolas.roldan@softvision.com" };

            context.Consultants.Add(consultant1);
            context.Consultants.Add(consultant2);
            context.Consultants.Add(consultant3);
            context.Consultants.Add(consultant4);
            context.Consultants.Add(consultant5);
            context.Consultants.Add(consultant6);
            context.Consultants.Add(consultant7);
            context.Consultants.Add(consultant8);
            context.Consultants.Add(consultant9);
            context.Consultants.Add(consultant10);
            context.Consultants.Add(consultant11);
            context.Consultants.Add(consultant12);
            context.Consultants.Add(consultant13);
            context.Consultants.Add(consultant14);
            context.Consultants.Add(consultant15);
            context.Consultants.Add(consultant19);
            context.Consultants.Add(consultant20);
            context.Consultants.Add(consultant21);
            context.Consultants.Add(consultant23);
            context.SaveChanges();
            #endregion

            #region Office
            var Office1 = new Office { Name = "Almagro", Description = "Almagro" };
            var Office2 = new Office { Name = "Vte Lopez", Description = "Vicente Lopez" };
            context.Office.Add(Office1);
            context.Office.Add(Office2);
            context.SaveChanges();
            #endregion

            #region Rooms
            var Room1 = new Room { Name = "Grande", Description = "Sala Grande", OfficeId = 2, Office = Office2 };
            var Room2 = new Room { Name = "Chica", Description = "Sala Chica", OfficeId = 2, Office = Office2 };
            var Room3 = new Room { Name = "Salita", Description = "Salita", OfficeId = 1, Office = Office1 };
            context.Room.Add(Room1);
            context.Room.Add(Room2);
            context.Room.Add(Room3);
            context.SaveChanges();
            #endregion


            #region Candidates
            var candidate1 = new Candidate { Profile=Profile1, Name = "Cristian", LastName = "Piqué", DNI = 34578644, PhoneNumber = "(+54)2355677581", EmailAddress = "cristian.pique@softvision.com", LinkedInProfile = "https://www.linkedin.com/in/me-cristianpique/", EnglishLevel = EnglishLevel.LowIntermediate, Status = CandidateStatus.InProgress, Recruiter = consultant1, PreferredOffice = Office1, Community=Community1};
            var candidate2 = new Candidate { Profile = Profile2, Community =Community2, Name = "Cristian", LastName = "Piqué", DNI = 1234567, PhoneNumber = "(+54)1122334455", EmailAddress = "gerardo.chechik@softvision.com", LinkedInProfile = "N/A", EnglishLevel = EnglishLevel.Advanced, Status = CandidateStatus.Hired, Recruiter = consultant2, PreferredOffice = Office2};
            var candidate3 = new Candidate { Profile = Profile3, Community = Community3, Name = "Javier", LastName = "Páez", DNI = 12345678, PhoneNumber = "(+54)1122334455", EmailAddress = "javier.paez@softvision.com", LinkedInProfile = "N/A", EnglishLevel = EnglishLevel.HighIntermediate, Status = CandidateStatus.Recall, Recruiter = consultant3, PreferredOffice = Office1};
            var candidate4 = new Candidate { Profile = Profile2, Community = Community4, Name = "Matías", LastName = "Luraghi", DNI = 1234578, PhoneNumber = "(+54)1122334455", EmailAddress = "matias.luraghi@softvision.com", LinkedInProfile = "N/A", EnglishLevel = EnglishLevel.LowIntermediate, Status = CandidateStatus.InProgress, Recruiter = consultant4, PreferredOffice = Office1};
            var candidate5 = new Candidate { Profile = Profile1, Community = Community1, Name = "Juan Pablo", LastName = "Maldonado", DNI = 1345678, PhoneNumber = "(+54)1122334455", EmailAddress = "juanpablo.maldonado@softvision.com", LinkedInProfile = "N/A", EnglishLevel = EnglishLevel.None, Status = CandidateStatus.InProgress, Recruiter = consultant5, PreferredOffice = Office2};
            var candidate6 = new Candidate { Profile = Profile3, Community = Community2, Name = "Matías", LastName = "Caniglia", DNI = 1234568, PhoneNumber = "(+1)1122334455", EmailAddress = "matias.caniglia@softvision.com", LinkedInProfile = "N/A", EnglishLevel = EnglishLevel.LowIntermediate, Status = CandidateStatus.InProgress, Recruiter = consultant6, PreferredOffice = Office2};
            var candidate7 = new Candidate { Profile = Profile2, Community = Community3, Name = "Gustavo", LastName = "Gilberto", DNI = 2345678, PhoneNumber = "(+54)1122334455", EmailAddress = "gustavo.gilberto@softvision.com", LinkedInProfile = "N/A", EnglishLevel = EnglishLevel.HighIntermediate, Status = CandidateStatus.Rejected, Recruiter = consultant1, PreferredOffice = Office2};
            var candidate8 = new Candidate { Profile = Profile1, Community = Community4, Name = "Pablo", LastName = "Gore", DNI = 345678, PhoneNumber = "(+54)1122334455", EmailAddress = "pablo.gore@softvision.com", LinkedInProfile = "N/A", EnglishLevel = EnglishLevel.LowIntermediate, Status = CandidateStatus.Rejected, Recruiter = consultant2, PreferredOffice = Office1};
            var candidate9 = new Candidate { Profile = Profile2, Community = Community1, Name = "Pablo", LastName = "Oubina", DNI = 12340678, PhoneNumber = "(+54)1122334455", EmailAddress = "pablo.oubina@softvision.com", LinkedInProfile = "N/A", EnglishLevel = EnglishLevel.Advanced, Status = CandidateStatus.InProgress, Recruiter = consultant3, PreferredOffice = Office1};
            var candidate10 = new Candidate { Profile = Profile3, Community = Community3, Name = "Martín", LastName = "Pielvitori", DNI = 10345678, PhoneNumber = "(+54)1122334455", EmailAddress = "martin.pielvitori@softvision.com", LinkedInProfile = "N/A", EnglishLevel = EnglishLevel.None, Status = CandidateStatus.Hired, Recruiter = consultant4, PreferredOffice = Office1};
            var candidate11 = new Candidate { Profile = Profile1, Community = Community2, Name = "Alejo", LastName = "Romano", DNI = 12345670, PhoneNumber = "(+1)1122334455", EmailAddress = "alejo.romano@softvision.com", LinkedInProfile = "N/A", EnglishLevel = EnglishLevel.HighIntermediate, Status = CandidateStatus.New, Recruiter = consultant5, PreferredOffice = Office1};
            context.Candidates.Add(candidate1);
            context.Candidates.Add(candidate2);
            context.Candidates.Add(candidate3);
            context.Candidates.Add(candidate4);
            context.Candidates.Add(candidate5);
            context.Candidates.Add(candidate6);
            context.Candidates.Add(candidate7);
            context.Candidates.Add(candidate8);
            context.Candidates.Add(candidate9);
            context.Candidates.Add(candidate10);
            context.Candidates.Add(candidate11);
            context.SaveChanges();
            #endregion

            #region Postulants
            var postulant1 = new Postulant { Name = "Ariel Ortega", EmailAddress = "ariel.ortega@hotmail.com", LinkedInProfile = "N/A", Cv = "N/A", CreatedDate = DateTime.Now };
            var postulant2 = new Postulant { Name = "Nicolas Pavlotsky", EmailAddress = "nicolas.pavlotsky@hotmail.com", LinkedInProfile = "N/A", Cv = "N/A", CreatedDate = DateTime.Now };
            var postulant3 = new Postulant { Name = "Rodrigo Ramirez", EmailAddress = "rodrigo.ramirez@hotmail.com", LinkedInProfile = "N/A", Cv = "N/A", CreatedDate = DateTime.Now };
            var postulant4 = new Postulant { Name = "Sebastian Liotta", EmailAddress = "seba.liotta@hotmail.com", LinkedInProfile = "N/A", Cv = "N/A", CreatedDate = DateTime.Now };

            context.Postulants.Add(postulant1);
            context.Postulants.Add(postulant2);
            context.Postulants.Add(postulant3);
            context.Postulants.Add(postulant4);
            context.SaveChanges();
            #endregion

            #region SkillType
            var skillType1 = new SkillType { Name = ".NET", Description = ".NET Framework, .NET Core, .NET languages" };
            var skillType2 = new SkillType { Name = "Java platform", Description = "Programming language and related frameworks and tools" };
            var skillType3 = new SkillType { Name = "Databases", Description = "Knowledge of databases" };
            var skillType4 = new SkillType { Name = "English", Description = "Level of english" };
            var skillType5 = new SkillType { Name = "Spanish", Description = "Level of spanish" };
            var skillType6 = new SkillType { Name = "Soft skills", Description = "Leadership, communication, teamwork..." };
            var skillType7 = new SkillType { Name = "Front-end", Description = "Javascript, HTML, HTTP" };
            var skillType8 = new SkillType { Name = "Back-end", Description = "Programming languages, frameworks, tools" };
            var skillType9 = new SkillType { Name = "DevOps", Description = "SysAdmin, Containers, Azure..." };
            var skillType10 = new SkillType { Name = "Project management", Description = "Project management" };
            var skillType11 = new SkillType { Name = "Mobile", Description = "Knowledge of mobile development" };
            context.SkillTypes.Add(skillType1);
            context.SkillTypes.Add(skillType2);
            context.SkillTypes.Add(skillType3);
            context.SkillTypes.Add(skillType4);
            context.SkillTypes.Add(skillType5);
            context.SkillTypes.Add(skillType6);
            context.SkillTypes.Add(skillType7);
            context.SkillTypes.Add(skillType8);
            context.SkillTypes.Add(skillType9);
            context.SkillTypes.Add(skillType10);
            context.SaveChanges();
            #endregion

            #region Skills
            var skill1a = new Skill { Name = "C#", Type = skillType1, Description = "Object-oriented programming language" };
            var skill1b = new Skill { Name = "F#", Type = skillType1, Description = "Functional programming language" };

            var skill2a = new Skill { Name = "Spring Framework", Type = skillType2, Description = "Application framework and inversion of control container for the Java platform" };

            var skill3a = new Skill { Name = "SQL server", Type = skillType3, Description = "Lenguaje de programación funcional" };
            var skill3b = new Skill { Name = "Oracle database", Type = skillType3, Description = "Lenguaje de programación funcional" };
            var skill3c = new Skill { Name = "MongoDB", Type = skillType3, Description = "NoSQL Database" };

            var skill4a = new Skill { Name = "Reading", Type = skillType4, Description = "Level of reading" };
            var skill4b = new Skill { Name = "Writing", Type = skillType4, Description = "Level of writing" };
            var skill4c = new Skill { Name = "Speaking", Type = skillType4, Description = "Level of speaking" };
            var skill4d = new Skill { Name = "Listening", Type = skillType4, Description = "Level of listening" };

            var skill5a = new Skill { Name = "Reading", Type = skillType5, Description = "Level of reading" };
            var skill5b = new Skill { Name = "Writing", Type = skillType5, Description = "Level of writing" };
            var skill5c = new Skill { Name = "Speaking", Type = skillType5, Description = "Level of speaking" };
            var skill5d = new Skill { Name = "Listening", Type = skillType5, Description = "Level of listening" };

            var skill6a = new Skill { Name = "Leadership", Type = skillType6, Description = "Leadership" };
            var skill6b = new Skill { Name = "Problem Solving", Type = skillType6, Description = "Problem Solving Skills" };
            var skill6c = new Skill { Name = "Teamwork", Type = skillType6, Description = "Teamwork" };
            var skill6d = new Skill { Name = "Work Ethic", Type = skillType6, Description = "Work Ethic" };

            var skill7a = new Skill { Name = "Javascript", Type = skillType7, Description = "Javascript" };
            var skill7b = new Skill { Name = "HTTP", Type = skillType7, Description = "HTTP" };
            var skill7c = new Skill { Name = "HTML5", Type = skillType7, Description = "Markup language" };

            var skill8a = new Skill { Name = "Entity framework", Type = skillType8, Description = "EF y EF Core" };
            var skill8b = new Skill { Name = "Async programming", Type = skillType8, Description = "Async" };
            var skill8c = new Skill { Name = "LINQ", Type = skillType8, Description = "LINQ" };

            var skill9a = new Skill { Name = "Azure", Type = skillType9, Description = "Microsoft's clould computing service" };

            var skill10a = new Skill { Name = "Leadership", Type = skillType10, Description = "PM skill" };
            var skill10b = new Skill { Name = "Team managment", Type = skillType10, Description = "PM skill" };

            var skill11a = new Skill { Name = "Xamarin", Type = skillType11, Description = "Multiplatform mobile development" };

            context.Skills.Add(skill1a);
            context.Skills.Add(skill1b);

            context.Skills.Add(skill2a);

            context.Skills.Add(skill3a);
            context.Skills.Add(skill3b);
            context.Skills.Add(skill3c);

            context.Skills.Add(skill4a);
            context.Skills.Add(skill4b);
            context.Skills.Add(skill4c);
            context.Skills.Add(skill4d);

            context.Skills.Add(skill5a);
            context.Skills.Add(skill5b);
            context.Skills.Add(skill5c);
            context.Skills.Add(skill5d);

            context.Skills.Add(skill6a);
            context.Skills.Add(skill6b);
            context.Skills.Add(skill6c);
            context.Skills.Add(skill6d);

            context.Skills.Add(skill7a);
            context.Skills.Add(skill7b);
            context.Skills.Add(skill7c);

            context.Skills.Add(skill8a);
            context.Skills.Add(skill8b);
            context.Skills.Add(skill8c);

            context.Skills.Add(skill9a);

            context.Skills.Add(skill10a);
            context.Skills.Add(skill10b);

            context.Skills.Add(skill11a);
            context.SaveChanges();
            #endregion

            #region CandidateSkill
            var cs1 = new CandidateSkill { CandidateId = 1, SkillId = 1, Rate = 85, Comment = "Knows a lot" };
            var cs2 = new CandidateSkill { CandidateId = 1, SkillId = 2, Rate = 100, Comment = "Knows everything" };
            var cs3 = new CandidateSkill { CandidateId = 1, SkillId = 3, Rate = 45, Comment = "Knows a little" };
            var cs4 = new CandidateSkill { CandidateId = 1, SkillId = 4, Rate = 85, Comment = "Knows a lot" };
            var cs5 = new CandidateSkill { CandidateId = 1, SkillId = 5, Rate = 100, Comment = "Knows everything" };
            var cs6 = new CandidateSkill { CandidateId = 1, SkillId = 6, Rate = 45, Comment = "Knows a little" };
            var cs7 = new CandidateSkill { CandidateId = 1, SkillId = 7, Rate = 85, Comment = "Knows a lot" };
            var cs8 = new CandidateSkill { CandidateId = 1, SkillId = 8, Rate = 100, Comment = "Knows everything" };
            var cs9 = new CandidateSkill { CandidateId = 1, SkillId = 9, Rate = 45, Comment = "Knows a little" };
            var cs10 = new CandidateSkill { CandidateId = 1, SkillId = 10, Rate = 40, Comment = "Knows a little" };
            var cs11 = new CandidateSkill { CandidateId = 1, SkillId = 11, Rate = 90, Comment = "Knows everything" };

            var cs1b = new CandidateSkill { CandidateId = 2, SkillId = 1, Rate = 85, Comment = "Knows a lot" };
            var cs2b = new CandidateSkill { CandidateId = 2, SkillId = 2, Rate = 100, Comment = "Knows everything" };
            var cs3b = new CandidateSkill { CandidateId = 2, SkillId = 3, Rate = 45, Comment = "Knows a little" };
            var cs4b = new CandidateSkill { CandidateId = 2, SkillId = 4, Rate = 85, Comment = "Knows a lot" };
            var cs5b = new CandidateSkill { CandidateId = 2, SkillId = 5, Rate = 100, Comment = "Knows everything" };
            var cs6b = new CandidateSkill { CandidateId = 2, SkillId = 6, Rate = 45, Comment = "Knows a little" };
            var cs7b = new CandidateSkill { CandidateId = 2, SkillId = 7, Rate = 85, Comment = "Knows a lot" };
            var cs8b = new CandidateSkill { CandidateId = 2, SkillId = 8, Rate = 100, Comment = "Knows everything" };
            var cs9b = new CandidateSkill { CandidateId = 2, SkillId = 9, Rate = 45, Comment = "Knows a little" };
            var cs10b = new CandidateSkill { CandidateId = 2, SkillId = 10, Rate = 40, Comment = "Knows a little" };
            var cs11b = new CandidateSkill { CandidateId = 2, SkillId = 11, Rate = 90, Comment = "Knows everything" };

            var cs1c = new CandidateSkill { CandidateId = 3, SkillId = 11, Rate = 85, Comment = "Knows a lot" };
            var cs2c = new CandidateSkill { CandidateId = 3, SkillId = 12, Rate = 100, Comment = "Knows everything" };
            var cs3c = new CandidateSkill { CandidateId = 3, SkillId = 13, Rate = 45, Comment = "Knows a little" };
            var cs4c = new CandidateSkill { CandidateId = 3, SkillId = 14, Rate = 85, Comment = "Knows a lot" };
            var cs5c = new CandidateSkill { CandidateId = 3, SkillId = 15, Rate = 100, Comment = "Knows everything" };
            var cs6c = new CandidateSkill { CandidateId = 3, SkillId = 16, Rate = 45, Comment = "Knows a little" };
            var cs7c = new CandidateSkill { CandidateId = 3, SkillId = 17, Rate = 85, Comment = "Knows a lot" };
            var cs8c = new CandidateSkill { CandidateId = 3, SkillId = 18, Rate = 100, Comment = "Knows everything" };
            var cs9c = new CandidateSkill { CandidateId = 3, SkillId = 19, Rate = 45, Comment = "Knows a little" };
            var cs10c = new CandidateSkill { CandidateId = 3, SkillId = 20, Rate = 40, Comment = "Knows a little" };
            var cs11c = new CandidateSkill { CandidateId = 3, SkillId = 21, Rate = 90, Comment = "Knows everything" };

            var cs1d = new CandidateSkill { CandidateId = 4, SkillId = 1, Rate = 85, Comment = "Knows a lot" };
            var cs2d = new CandidateSkill { CandidateId = 4, SkillId = 3, Rate = 100, Comment = "Knows everything" };
            var cs3d = new CandidateSkill { CandidateId = 4, SkillId = 4, Rate = 45, Comment = "Knows a little" };
            var cs4d = new CandidateSkill { CandidateId = 4, SkillId = 6, Rate = 85, Comment = "Knows a lot" };
            var cs5d = new CandidateSkill { CandidateId = 4, SkillId = 8, Rate = 100, Comment = "Knows everything" };
            var cs6d = new CandidateSkill { CandidateId = 4, SkillId = 10, Rate = 45, Comment = "Knows a little" };
            var cs7d = new CandidateSkill { CandidateId = 4, SkillId = 12, Rate = 85, Comment = "Knows a lot" };
            var cs8d = new CandidateSkill { CandidateId = 4, SkillId = 14, Rate = 100, Comment = "Knows everything" };
            var cs9d = new CandidateSkill { CandidateId = 4, SkillId = 16, Rate = 45, Comment = "Knows a little" };
            var cs10d = new CandidateSkill { CandidateId = 4, SkillId = 18, Rate = 40, Comment = "Knows a little" };
            var cs11d = new CandidateSkill { CandidateId = 4, SkillId = 20, Rate = 90, Comment = "Knows everything" };

            var cs1g = new CandidateSkill { CandidateId = 7, SkillId = 1, Rate = 85, Comment = "Knows a lot" };
            var cs2g = new CandidateSkill { CandidateId = 7, SkillId = 3, Rate = 100, Comment = "Knows everything" };
            var cs3g = new CandidateSkill { CandidateId = 7, SkillId = 4, Rate = 45, Comment = "Knows a little" };
            var cs4g = new CandidateSkill { CandidateId = 7, SkillId = 6, Rate = 85, Comment = "Knows a lot" };
            var cs5g = new CandidateSkill { CandidateId = 7, SkillId = 8, Rate = 100, Comment = "Knows everything" };
            var cs6g = new CandidateSkill { CandidateId = 7, SkillId = 10, Rate = 45, Comment = "Knows a little" };
            var cs7g = new CandidateSkill { CandidateId = 7, SkillId = 12, Rate = 85, Comment = "Knows a lot" };
            var cs8g = new CandidateSkill { CandidateId = 7, SkillId = 14, Rate = 100, Comment = "Knows everything" };
            var cs9g = new CandidateSkill { CandidateId = 7, SkillId = 16, Rate = 45, Comment = "Knows a little" };
            var cs10g = new CandidateSkill { CandidateId = 7, SkillId = 18, Rate = 40, Comment = "Knows a little" };
            var cs11g = new CandidateSkill { CandidateId = 7, SkillId = 20, Rate = 90, Comment = "Knows everything" };

            var cs1e = new CandidateSkill { CandidateId = 8, SkillId = 1, Rate = 85, Comment = "Knows a lot" };
            var cs2e = new CandidateSkill { CandidateId = 8, SkillId = 3, Rate = 100, Comment = "Knows everything" };
            var cs3e = new CandidateSkill { CandidateId = 8, SkillId = 4, Rate = 45, Comment = "Knows a little" };
            var cs4e = new CandidateSkill { CandidateId = 8, SkillId = 6, Rate = 85, Comment = "Knows a lot" };
            var cs5e = new CandidateSkill { CandidateId = 8, SkillId = 8, Rate = 100, Comment = "Knows everything" };
            var cs6e = new CandidateSkill { CandidateId = 8, SkillId = 10, Rate = 45, Comment = "Knows a little" };
            var cs7e = new CandidateSkill { CandidateId = 8, SkillId = 12, Rate = 85, Comment = "Knows a lot" };
            var cs8e = new CandidateSkill { CandidateId = 8, SkillId = 14, Rate = 100, Comment = "Knows everything" };
            var cs9e = new CandidateSkill { CandidateId = 8, SkillId = 16, Rate = 45, Comment = "Knows a little" };
            var cs10e = new CandidateSkill { CandidateId = 8, SkillId = 18, Rate = 40, Comment = "Knows a little" };
            var cs11e = new CandidateSkill { CandidateId = 8, SkillId = 20, Rate = 90, Comment = "Knows everything" };

            var cs1f = new CandidateSkill { CandidateId = 9, SkillId = 8, Rate = 85, Comment = "Knows a lot" };
            var cs2f = new CandidateSkill { CandidateId = 9, SkillId = 10, Rate = 100, Comment = "Knows everything" };
            var cs3f = new CandidateSkill { CandidateId = 9, SkillId = 12, Rate = 45, Comment = "Knows a little" };
            var cs4f = new CandidateSkill { CandidateId = 9, SkillId = 14, Rate = 85, Comment = "Knows a lot" };
            var cs5f = new CandidateSkill { CandidateId = 9, SkillId = 16, Rate = 100, Comment = "Knows everything" };
            var cs6f = new CandidateSkill { CandidateId = 9, SkillId = 18, Rate = 45, Comment = "Knows a little" };
            var cs7f = new CandidateSkill { CandidateId = 9, SkillId = 20, Rate = 85, Comment = "Knows a lot" };
            var cs8f = new CandidateSkill { CandidateId = 9, SkillId = 22, Rate = 100, Comment = "Knows everything" };
            var cs9f = new CandidateSkill { CandidateId = 9, SkillId = 24, Rate = 45, Comment = "Knows a little" };
            var cs10f = new CandidateSkill { CandidateId = 9, SkillId = 26, Rate = 40, Comment = "Knows a little" };
            var cs11f = new CandidateSkill { CandidateId = 9, SkillId = 27, Rate = 90, Comment = "Knows everything" };


            context.CandidateSkills.Add(cs1);
            context.CandidateSkills.Add(cs2);
            context.CandidateSkills.Add(cs3);
            context.CandidateSkills.Add(cs4);
            context.CandidateSkills.Add(cs5);
            context.CandidateSkills.Add(cs6);
            context.CandidateSkills.Add(cs7);
            context.CandidateSkills.Add(cs8);
            context.CandidateSkills.Add(cs9);
            context.CandidateSkills.Add(cs10);
            context.CandidateSkills.Add(cs11);

            context.CandidateSkills.Add(cs1b);
            context.CandidateSkills.Add(cs2b);
            context.CandidateSkills.Add(cs3b);
            context.CandidateSkills.Add(cs4b);
            context.CandidateSkills.Add(cs5b);
            context.CandidateSkills.Add(cs6b);
            context.CandidateSkills.Add(cs7b);
            context.CandidateSkills.Add(cs8b);
            context.CandidateSkills.Add(cs9b);
            context.CandidateSkills.Add(cs10b);
            context.CandidateSkills.Add(cs11b);

            context.CandidateSkills.Add(cs1c);
            context.CandidateSkills.Add(cs2c);
            context.CandidateSkills.Add(cs3c);
            context.CandidateSkills.Add(cs4c);
            context.CandidateSkills.Add(cs5c);
            context.CandidateSkills.Add(cs6c);
            context.CandidateSkills.Add(cs7c);
            context.CandidateSkills.Add(cs8c);
            context.CandidateSkills.Add(cs9c);
            context.CandidateSkills.Add(cs10c);
            context.CandidateSkills.Add(cs11c);

            context.CandidateSkills.Add(cs1d);
            context.CandidateSkills.Add(cs2d);
            context.CandidateSkills.Add(cs3d);
            context.CandidateSkills.Add(cs4d);
            context.CandidateSkills.Add(cs5d);
            context.CandidateSkills.Add(cs6d);
            context.CandidateSkills.Add(cs7d);
            context.CandidateSkills.Add(cs8d);
            context.CandidateSkills.Add(cs9d);
            context.CandidateSkills.Add(cs10d);
            context.CandidateSkills.Add(cs11d);

            context.CandidateSkills.Add(cs1e);
            context.CandidateSkills.Add(cs2e);
            context.CandidateSkills.Add(cs3e);
            context.CandidateSkills.Add(cs4e);
            context.CandidateSkills.Add(cs5e);
            context.CandidateSkills.Add(cs6e);
            context.CandidateSkills.Add(cs7e);
            context.CandidateSkills.Add(cs8e);
            context.CandidateSkills.Add(cs9e);
            context.CandidateSkills.Add(cs10e);
            context.CandidateSkills.Add(cs11e);

            context.CandidateSkills.Add(cs1f);
            context.CandidateSkills.Add(cs2f);
            context.CandidateSkills.Add(cs3f);
            context.CandidateSkills.Add(cs4f);
            context.CandidateSkills.Add(cs5f);
            context.CandidateSkills.Add(cs6f);
            context.CandidateSkills.Add(cs7f);
            context.CandidateSkills.Add(cs8f);
            context.CandidateSkills.Add(cs9f);
            context.CandidateSkills.Add(cs10f);
            context.CandidateSkills.Add(cs11f);

            context.CandidateSkills.Add(cs1g);
            context.CandidateSkills.Add(cs2g);
            context.CandidateSkills.Add(cs3g);
            context.CandidateSkills.Add(cs4g);
            context.CandidateSkills.Add(cs5g);
            context.CandidateSkills.Add(cs6g);
            context.CandidateSkills.Add(cs7g);
            context.CandidateSkills.Add(cs8g);
            context.CandidateSkills.Add(cs9g);
            context.CandidateSkills.Add(cs10g);
            context.CandidateSkills.Add(cs11g);

            context.SaveChanges();
            #endregion

            #region Process
            var process1 = new Process
            {
                CandidateId = 1,
                ConsultantDelegateId = 1,
                ConsultantOwnerId = 2,     
                StartDate = DateTime.Today.AddDays(-100),
                Status = ProcessStatus.InProgress,
                CurrentStage = ProcessCurrentStage.ClientStage,
                HrStage = new HrStage { ProcessId = 1, ConsultantDelegateId = 1, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(-14), EnglishLevel = EnglishLevel.HighIntermediate, WantedSalary = 10000, ActualSalary = 5000 },
                TechnicalStage = new TechnicalStage { ProcessId = 1, ConsultantDelegateId = 2, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(-12), Seniority = Seniority.SemiSenior2, Client = "EY" },
                ClientStage = new ClientStage { ProcessId = 1, ConsultantDelegateId = 2, Interviewer = "", ConsultantOwnerId = 1, DelegateName = "", Status = StageStatus.InProgress, Date = DateTime.Today.AddDays(-12) },
                OfferStage = new OfferStage { ProcessId = 1, ConsultantDelegateId = 4, ConsultantOwnerId = 1, Status = StageStatus.NA, Date = DateTime.Today.AddDays(-12), HireDate = DateTime.Today.AddDays(-12), Seniority = Seniority.Senior1 }
            };

            var process2 = new Process
            {
                CandidateId = 2,
                ConsultantDelegateId = 3,
                ConsultantOwnerId = 4,
                StartDate = DateTime.Today.AddDays(-80),
                Status = ProcessStatus.InProgress,
                CurrentStage = ProcessCurrentStage.ClientStage,
                HrStage = new HrStage { ProcessId = 2, ConsultantDelegateId = 1, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(-14) },
                TechnicalStage = new TechnicalStage { ProcessId = 2, ConsultantDelegateId = 2, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(-12), Client = "PWC" },
                ClientStage = new ClientStage { ProcessId = 2, ConsultantDelegateId = 2, ConsultantOwnerId = 1, Status = StageStatus.InProgress, Date = DateTime.Today.AddDays(-12) },
                OfferStage = new OfferStage { ProcessId = 2, ConsultantDelegateId = 4, ConsultantOwnerId = 1, Status = StageStatus.NA, Date = DateTime.Today.AddDays(-12), HireDate = DateTime.Today.AddDays(-10) }
            };

            var process3 = new Process
            {
                CandidateId = 7,
                ConsultantDelegateId = 4,
                ConsultantOwnerId = 5,
                StartDate = DateTime.Today.AddDays(-40),
                EndDate = DateTime.Today.AddDays(-10),
                Status = ProcessStatus.Rejected,
                CurrentStage = ProcessCurrentStage.Finished,
                HrStage = new HrStage { ProcessId = 3, ConsultantDelegateId = 1, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(-14) },
                TechnicalStage = new TechnicalStage { ProcessId = 3, ConsultantDelegateId = 2, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(-12), Client = "YL" },
                ClientStage = new ClientStage { ProcessId = 3, ConsultantDelegateId = 2, ConsultantOwnerId = 1, Status = StageStatus.Rejected, Date = DateTime.Today.AddDays(-12) },
                OfferStage = new OfferStage { ProcessId = 3, ConsultantDelegateId = 4, ConsultantOwnerId = 1, Status = StageStatus.NA, Date = DateTime.Today.AddDays(-12), HireDate = DateTime.Today.AddDays(-1) }
            };

            var process4 = new Process
            {
                CandidateId = 4,
                ConsultantDelegateId = 6,
                ConsultantOwnerId = 1,
                StartDate = DateTime.Today.AddDays(-5),
                Status = ProcessStatus.InProgress,
                CurrentStage = ProcessCurrentStage.ClientStage,
                HrStage = new HrStage { ProcessId = 4, ConsultantDelegateId = 1, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(-14) },
                TechnicalStage = new TechnicalStage { ProcessId = 4, ConsultantDelegateId = 2, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(-12), Client = "" },
                ClientStage = new ClientStage { ProcessId = 4, ConsultantDelegateId = 2, ConsultantOwnerId = 1, Status = StageStatus.NA, Date = DateTime.Today.AddDays(-12) },
                OfferStage = new OfferStage { ProcessId = 4, ConsultantDelegateId = 4, ConsultantOwnerId = 1, Status = StageStatus.NA, Date = DateTime.Today.AddDays(-12), HireDate = DateTime.Today.AddDays(15) }
            };

            var process5 = new Process
            {
                CandidateId = 5,
                ConsultantDelegateId = 1,
                ConsultantOwnerId = 4,
                StartDate = DateTime.Today.AddDays(-1),
                Status = ProcessStatus.Hired,
                CurrentStage = ProcessCurrentStage.Finished,
                HrStage = new HrStage { ProcessId = 5, ConsultantDelegateId = 1, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(12) },
                TechnicalStage = new TechnicalStage { ProcessId = 5, ConsultantDelegateId = 2, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(-12), Client = "" },
                ClientStage = new ClientStage { ProcessId = 5, ConsultantDelegateId = 2, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(-12) },
                OfferStage = new OfferStage { ProcessId = 5, ConsultantDelegateId = 4, ConsultantOwnerId = 1, Status = StageStatus.Hired, Date = DateTime.Today.AddDays(-12), HireDate = DateTime.Today.AddDays(35) }
        };

            var process6 = new Process
            {
                CandidateId = 6,
                ConsultantDelegateId = 5,
                ConsultantOwnerId = 3,
                StartDate = DateTime.Today.AddDays(-25),
                Status = ProcessStatus.Recall,
                CurrentStage = ProcessCurrentStage.ClientStage,
                HrStage = new HrStage { ProcessId = 6, ConsultantDelegateId = 1, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(-14) },
                TechnicalStage = new TechnicalStage { ProcessId = 6, ConsultantDelegateId = 2, ConsultantOwnerId = 1, Status = StageStatus.Accepted, Date = DateTime.Today.AddDays(-12), Client = "EY" },
                ClientStage = new ClientStage { ProcessId = 6, ConsultantDelegateId = 2, ConsultantOwnerId = 1, Status = StageStatus.InProgress, Date = DateTime.Today.AddDays(-12) },
                OfferStage = new OfferStage { ProcessId = 6, ConsultantDelegateId = 4, ConsultantOwnerId = 1, Status = StageStatus.NA, Date = DateTime.Today.AddDays(-12), HireDate = DateTime.Today.AddDays(3) }
            };

            context.Processes.Add(process1);
            context.Processes.Add(process2);
            context.Processes.Add(process3);
            context.Processes.Add(process4);
            context.Processes.Add(process5);
            context.Processes.Add(process6);
            context.SaveChanges();
            #endregion

            #region Tasks
            var task1 = new Task { Title = "Búsqueda de DevOps", CreatedDate = DateTime.Now.AddDays(-7), EndDate = DateTime.Now.AddDays(-1), ConsultantId = 1, IsApprove = false };
            var task2 = new Task { Title = "Contactar candidato José Pérez", CreatedDate = DateTime.Now.AddDays(-2), EndDate = DateTime.Now.AddDays(1), ConsultantId = 2, IsApprove = false };
            var task3 = new Task { Title = "Envío de mail con feriados", CreatedDate = DateTime.Now, EndDate = DateTime.Now.AddDays(11), ConsultantId = 3, IsApprove = false, IsNew = true };
            var task4 = new Task { Title = "Búsqueda de .Net", CreatedDate = DateTime.Now.AddDays(-7), EndDate = DateTime.Now.AddDays(-1), ConsultantId = 7, IsApprove = false, IsNew = false };
            var task5 = new Task { Title = "Contactar candidato Juan", CreatedDate = DateTime.Now.AddDays(-2), EndDate = DateTime.Now.AddDays(1), ConsultantId = 7, IsApprove = false, IsNew = false };
            var task6 = new Task { Title = "Envío de mail con WHF", CreatedDate = DateTime.Now, EndDate = DateTime.Now.AddDays(11), ConsultantId = 7, IsApprove = false, IsNew = true };
            context.Tasks.Add(task1);
            context.Tasks.Add(task2);
            context.Tasks.Add(task3);
            context.Tasks.Add(task4);
            context.Tasks.Add(task5);
            context.Tasks.Add(task6);
            context.SaveChanges();
            #endregion

            #region TasktItems
            var taskItem1 = new TaskItem { Text = "Búsqueda en LinkedIn", TaskId = 1, Checked = false, Task = task1 };
            var taskItem2 = new TaskItem { Text = "Contacto con candidatos", TaskId = 1, Checked = false, Task = task1 };
            var taskItem3 = new TaskItem { Text = "Elección de 3 candidatos más sobresalientes", TaskId = 1, Checked = false, Task = task1 };
            var taskItem4 = new TaskItem { Text = "Agendar entrevistas", Checked = false, Task = task1 };
            var taskItem5 = new TaskItem { Text = "Agendar entrevista", TaskId = 2, Checked = false, Task = task2 };
            var taskItem6 = new TaskItem { Text = "Confección de oferta", TaskId = 2, Checked = false, Task = task2 };
            var taskItem7 = new TaskItem { Text = "Acordar elección de banco y vacaciones", TaskId = 2, Checked = false, Task = task2 };
            var taskItem8 = new TaskItem { Text = "Acordar fecha de ingreso", TaskId = 2, Checked = false, Task = task2 };
            var taskItem9 = new TaskItem { Text = "Confeccionar mail", TaskId = 3, Checked = false, Task = task3 };
            var taskItem10 = new TaskItem { Text = "Crear lista de destinatarios", TaskId = 3, Checked = false, Task = task3 };
            var taskItem11 = new TaskItem { Text = "Validar lista de feriados con HR", TaskId = 3, Checked = false, Task = task3 };
            var taskItem12 = new TaskItem { Text = "Envío de mail con feriados", TaskId = 3, Checked = false, Task = task3 };


            context.TaskItems.Add(taskItem1);
            context.TaskItems.Add(taskItem2);
            context.TaskItems.Add(taskItem3);
            context.TaskItems.Add(taskItem4);
            context.TaskItems.Add(taskItem5);
            context.TaskItems.Add(taskItem6);
            context.TaskItems.Add(taskItem7);
            context.TaskItems.Add(taskItem8);
            context.TaskItems.Add(taskItem9);
            context.TaskItems.Add(taskItem10);
            context.TaskItems.Add(taskItem11);
            context.TaskItems.Add(taskItem12);
            context.SaveChanges();
            #endregion

            #region HireProjection
            var hProjection1 = new HireProjection { Month = DateTime.Now.Month - 1, Year = DateTime.Now.Year, Value = 65};
            var hProjection2 = new HireProjection { Month = DateTime.Now.Month, Year = DateTime.Now.Year, Value = 35 };
            var hProjection3 = new HireProjection { Month = DateTime.Now.Month + 1, Year = DateTime.Now.Year, Value = 85 };
            context.HireProjection.Add(hProjection1);
            context.HireProjection.Add(hProjection2);
            context.HireProjection.Add(hProjection3);
            context.SaveChanges();
            #endregion

            #region EmployeeCasualty
            var eCasualty1 = new EmployeeCasualty { Month = DateTime.Now.Month - 1, Year = DateTime.Now.Year, Value = 45 };
            var eCasualty2 = new EmployeeCasualty { Month = DateTime.Now.Month, Year = DateTime.Now.Year, Value = 22 };
            var eCasualty3 = new EmployeeCasualty { Month = DateTime.Now.Month + 1, Year = DateTime.Now.Year, Value = 68 };
            context.EmployeeCasualty.Add(eCasualty1);
            context.EmployeeCasualty.Add(eCasualty2);
            context.EmployeeCasualty.Add(eCasualty3);
            context.SaveChanges();
            #endregion

            #region Role
            var Role1 = new Role { Name = "Junior Software Engenering", isActive = true };
            var Role2 = new Role { Name = "UX Developer Senior ", isActive = true };
            context.Roles.Add(Role1);
            context.Roles.Add(Role2);
            #endregion

            #region User
            var user1 = new User { FirstName = "Cristian", LastName = "Pique", Username = "cristian.pique@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user2 = new User { FirstName = "Emanuel", LastName = "Flores", Username = "emanuel.flores@softvision.com", Role = Roles.HRManagement, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user3 = new User { FirstName = "Ignacio", LastName = "Costantini", Username = "ignacio.costantini@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user4 = new User { FirstName = "Damian", LastName = "Fernandez Urroz", Username = "damian.fernandezurroz@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user5 = new User { FirstName = "Lucas", LastName = "Stirpe", Username = "lucas.stirpe@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user6 = new User { FirstName = "Adrian", LastName = "Rodriguez Renda", Username = "adrian.rodriguezrenda@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user7 = new User { FirstName = "Kevin", LastName = "Zatel", Username = "kevin.zatel@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user8 = new User { FirstName = "Tomas", LastName = "Rebollo", Username = "tomas.rebollo@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user9 = new User { FirstName = "Matias", LastName = "Zatz", Username = "matias.zatz@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user10 = new User { FirstName = "Thomas", LastName = "Nazar", Username = "thomas.nazar@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user11 = new User { FirstName = "Ivan", LastName = "Cardozo", Username = "ivan.cardozo@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user12 = new User { FirstName = "Karen", LastName = "Ono", Username = "karen.ono@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user13 = new User { FirstName = "Javier", LastName = "Benavente", Username = "javier.benavente@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user14 = new User { FirstName = "Facundo", LastName = "Valeriano", Username = "facundo.valeriano@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user15 = new User { FirstName = "Matias", LastName = "Otero", Username = "matias.otero@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user16 = new User { FirstName = "Matias", LastName = "Totaro", Username = "matias.totaro@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user17 = new User { FirstName = "Francisco", LastName = "Ghersi", Username = "francisco.ghersi@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user18 = new User { FirstName = "Victor", LastName = "Hidalgo", Username = "victor.hidalgo@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user19 = new User { FirstName = "Teo", LastName = "Benavides", Username = "teo.benavides@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user20 = new User { FirstName = "Tomas", LastName = "Guagnini", Username = "tomas.guagnini@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user21 = new User { FirstName = "Ignacio", LastName = "Casciaro", Username = "ignacio.casciaro@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user22 = new User { FirstName = "Tomas", LastName = "Reisenman", Username = "tomas.reisenman@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4",Community=Community2};

            // Users for testing Roles 
            //var user99 = new User { FirstName = "root", LastName = "root", Username = "root@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            //var user90 = new User { FirstName = "communitymanager", LastName = "communitymanager", Username = "communitymanager@softvision.com", Role = Roles.CommunityManager, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4",Community=Community1};
            //var user91 = new User { FirstName = "recruiter", LastName = "recruiter", Username = "recruiter@softvision.com", Role = Roles.Recruiter, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            //var user92 = new User { FirstName = "common", LastName = "common", Username = "common@softvision.com", Role = Roles.Common, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            //var user93 = new User { FirstName = "interviewer", LastName = "interviewer", Username = "interviewer@softvision.com", Role = Roles.Interviewer, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };



            var user22 = new User { FirstName = "Tomas", LastName = "Reisenman", Username = "tomas.reisenman@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user23 = new User { FirstName = "Nicolas", LastName = "Roldan", Username = "nicolas.roldan@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            var user24 = new User { FirstName = "Rodrigo", LastName = "Ramírez", Username = "rodrigo.ramirez@softvision.com", Role = Roles.Admin, Token = "", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            
            context.Users.Add(user2);
            context.Users.Add(user3);
            context.Users.Add(user4);
            context.Users.Add(user5);
            context.Users.Add(user6);
            context.Users.Add(user7);
            context.Users.Add(user8);
            context.Users.Add(user9);
            context.Users.Add(user10);
            context.Users.Add(user11);
            context.Users.Add(user12);
            context.Users.Add(user13);
            context.Users.Add(user14);
            context.Users.Add(user15);
            context.Users.Add(user16);
            context.Users.Add(user17);
            context.Users.Add(user18);
            context.Users.Add(user19);
            context.Users.Add(user20);
            context.Users.Add(user21);
            context.Users.Add(user22);
            //context.Users.Add(user99);
            //context.Users.Add(user90);
            //context.Users.Add(user91);
            //context.Users.Add(user92);
            //context.Users.Add(user93);
            context.Users.Add(user23);
            context.Users.Add(user24);
            
            context.SaveChanges();
            #endregion

            #region Employees
            var employee1 = new Employee { Name = "Without", LastName = "Reviewer" };
            var employee2 = new Employee { Name = "Cristian", LastName = "Rodríguez", DNI = 38888888, PhoneNumber = "(+54)12312312", EmailAddress = "critian.rodriguez@softvision.com", LinkedInProfile = "cristianlinkedin.com", AdditionalInformation = "Some aditional", Recruiter = consultant1, Role = Role2, isReviewer = true, Status = EmployeeStatus.Hired, Reviewer = employee1 };
            var employee3 = new Employee { Name = "Kevin", LastName = "Zatel", DNI = 38999999, PhoneNumber = "(+54)32132132", EmailAddress = "kevin.zatel@softvision.com", LinkedInProfile = "kevinlinkedin.com", AdditionalInformation = "Some aditional", Recruiter = consultant2, Role = Role1, isReviewer = false, Reviewer = employee2, Status = EmployeeStatus.Hired };
            var employee4 = new Employee { Name = "Tomas", LastName = "Rebollo", DNI = 40000000, PhoneNumber = "(+54)32132132", EmailAddress = "tomas.rebollo@softvision.com", LinkedInProfile = "kevinlinkedin.com", AdditionalInformation = "Some aditional", Recruiter = consultant2, Role = Role1, isReviewer = false, Reviewer = employee2, Status = EmployeeStatus.Hired };
            var employee5 = new Employee { Name = "Matias", LastName = "Zatz", DNI = 41111111, PhoneNumber = "(+54)32132132", EmailAddress = "matias.zatz@softvision.com", LinkedInProfile = "kevinlinkedin.com", AdditionalInformation = "Some aditional", Recruiter = consultant2, Role = Role1, isReviewer = false, Reviewer = employee2, Status = EmployeeStatus.Hired };
            var employee6 = new Employee { Name = "Thomas", LastName = "Nazar", DNI = 42222222, PhoneNumber = "(+54)32132132", EmailAddress = "thomas.nazar@softvision.com", LinkedInProfile = "kevinlinkedin.com", AdditionalInformation = "Some aditional", Recruiter = consultant2, Role = Role1, isReviewer = false, Reviewer = employee2, Status = EmployeeStatus.Hired };
            var employee7 = new Employee { Name = "Ivan", LastName = "Cardozo", DNI = 43333333, PhoneNumber = "(+54)32132132", EmailAddress = "thomas.nazar@softvision.com", LinkedInProfile = "kevinlinkedin.com", AdditionalInformation = "Some aditional", Recruiter = consultant2, Role = Role1, isReviewer = false, Reviewer = employee2, Status = EmployeeStatus.Hired };
            var employee8 = new Employee { Name = "Karen", LastName = "Ono", DNI = 44444444, PhoneNumber = "(+54)32132132", EmailAddress = "thomas.nazar@softvision.com", LinkedInProfile = "kevinlinkedin.com", AdditionalInformation = "Some aditional", Recruiter = consultant2, Role = Role1, isReviewer = false, Reviewer = employee2, Status = EmployeeStatus.Hired };
            var employee9 = new Employee { Name = "Javier", LastName = "Benavente", DNI = 42222223, PhoneNumber = "(+54)32132132", EmailAddress = "thomas.nazar@softvision.com", LinkedInProfile = "kevinlinkedin.com", AdditionalInformation = "Some aditional", Recruiter = consultant2, Role = Role1, isReviewer = false, Reviewer = employee2, Status = EmployeeStatus.Hired };
            var employee10 = new Employee { Name = "Facundo", LastName = "Valeriano", DNI = 42222278, PhoneNumber = "(+54)32132132", EmailAddress = "thomas.nazar@softvision.com", LinkedInProfile = "kevinlinkedin.com", AdditionalInformation = "Some aditional", Recruiter = consultant2, Role = Role1, isReviewer = false, Reviewer = employee2, Status = EmployeeStatus.Hired };
            var employee11 = new Employee { Name = "Matias", LastName = "Otero", DNI = 42222452, PhoneNumber = "(+54)32132132", EmailAddress = "thomas.nazar@softvision.com", LinkedInProfile = "kevinlinkedin.com", AdditionalInformation = "Some aditional", Recruiter = consultant2, Role = Role1, isReviewer = false, Reviewer = employee2, Status = EmployeeStatus.Hired };
            var employee12 = new Employee { Name = "Matias", LastName = "Totaro", DNI = 42222452, PhoneNumber = "(+54)32132132", EmailAddress = "matias.totaro@softvision.com", LinkedInProfile = "kevinlinkedin.com", AdditionalInformation = "Some aditional", Recruiter = consultant2, Role = Role1, isReviewer = false, Reviewer = employee2, Status = EmployeeStatus.Hired };

            context.Employees.Add(employee1);
            context.Employees.Add(employee2);
            context.Employees.Add(employee3);
            context.Employees.Add(employee4);
            context.Employees.Add(employee5);
            context.Employees.Add(employee6);
            context.Employees.Add(employee7);
            context.Employees.Add(employee8);
            context.Employees.Add(employee9);
            context.Employees.Add(employee10);
            context.Employees.Add(employee11);
            context.Employees.Add(employee12);

            context.SaveChanges();
            #endregion

            #region Dashboards
            var dashboard1 = new Dashboard { Name = "Processes Dashboard" };
            var dashboard2 = new Dashboard { Name = "Top 3 Known Skills Dashboard" }; 
            var dashboard3 = new Dashboard { Name = "Progress Processes Dashboard" };
            var dashboard4 = new Dashboard { Name = "Completed Processes Dashboard" };
            var dashboard5 = new Dashboard { Name = "Casualties and actual hires comparation Dashboard" };
            var dashboard6 = new Dashboard { Name = "Hire Projection Dashboard" };
            var dashboard7 = new Dashboard { Name = "Average: Interview date - Offer Accepted Dashboard" };
            var dashboard8 = new Dashboard { Name = "Average: Interview date - Hire Date Dashboard" };
            var dashboard9 = new Dashboard { Name = "Decline Reasons Dashboard" };
            var dashboard10 = new Dashboard { Name = "Weekly Candidates Dashboard" };

            context.Dashboards.Add(dashboard1);
            context.Dashboards.Add(dashboard2);
            context.Dashboards.Add(dashboard3);
            context.Dashboards.Add(dashboard4);
            context.Dashboards.Add(dashboard5);
            context.Dashboards.Add(dashboard6);
            context.Dashboards.Add(dashboard7);
            context.Dashboards.Add(dashboard8);
            context.Dashboards.Add(dashboard9);
            context.Dashboards.Add(dashboard10);

            context.SaveChanges();
            #endregion

            #region DaysOff
            var daysOff1 = new DaysOff {  Date = DateTime.Today.AddDays(-12), EndDate= DateTime.Today.AddDays(-12), Type = DaysOffType.Holidays, Status = DaysOffStatus.InReview, EmployeeId = 2, Employee = employee2 };
            var daysOff2 = new DaysOff {  Date = DateTime.Today.AddDays(-14), EndDate = DateTime.Today.AddDays(-14), Type = DaysOffType.Training, Status = DaysOffStatus.Accepted, EmployeeId = 1, Employee = employee1 };
            var daysOff3 = new DaysOff {  Date = DateTime.Today.AddDays(-16), EndDate = DateTime.Today.AddDays(-16), Type = DaysOffType.Training, Status = DaysOffStatus.InReview, EmployeeId = 2, Employee = employee2 };
            var daysOff4 = new DaysOff {  Date = DateTime.Today.AddDays(-18), EndDate = DateTime.Today.AddDays(-18), Type = DaysOffType.Holidays, Status = DaysOffStatus.Accepted, EmployeeId = 1, Employee = employee1 };
            var daysOff5 = new DaysOff { Date = DateTime.Today.AddDays(-18), EndDate = DateTime.Today.AddDays(-18), Type = DaysOffType.Holidays, Status = DaysOffStatus.InReview, EmployeeId = 4, Employee = employee4 };
            var daysOff6 = new DaysOff { Date = DateTime.Today.AddDays(-18), EndDate = DateTime.Today.AddDays(-18), Type = DaysOffType.PTO, Status = DaysOffStatus.InReview, EmployeeId = 4, Employee = employee4 };
            context.DaysOff.Add(daysOff1);
            context.DaysOff.Add(daysOff2);
            context.DaysOff.Add(daysOff3);
            context.DaysOff.Add(daysOff4);
            context.DaysOff.Add(daysOff5);
            context.DaysOff.Add(daysOff6);
            context.SaveChanges();
            #endregion

            #region CompanyCalendar
            var companyCalendar1 = new CompanyCalendar { Date = DateTime.Today.AddDays(-12), Type = "Festivity", Comments = "comenntsone" };
            var companyCalendar2 = new CompanyCalendar { Date = DateTime.Today.AddDays(-14), Type = "Reminder", Comments = "comenntstwo" };
            var companyCalendar3 = new CompanyCalendar { Date = DateTime.Today.AddDays(-16), Type = "Festivity", Comments = "comenntsthree" };
            var companyCalendar4 = new CompanyCalendar { Date = DateTime.Today.AddDays(-18), Type = "Festivity", Comments = "comenntsfour" };
            //ejemplo puede fallar en el edit porque date es de tipo ISOString: arreglar
            context.CompanyCalendar.Add(companyCalendar1);
            context.CompanyCalendar.Add(companyCalendar2);
            context.CompanyCalendar.Add(companyCalendar3);
            context.CompanyCalendar.Add(companyCalendar4);
            context.SaveChanges();
            #endregion


            #region Reservation
            var Reservation1 = new Reservation { Description = "Comunidad de .Net", RoomId = 1, Room = Room1, SinceReservation = new DateTime(2019, 8, 1, 10, 47, 0), UntilReservation = new DateTime(2019, 8, 1, 10, 50, 0), Recruiter = consultant1 };
            var Reservation2 = new Reservation { Description = "Comunidad de Developers", RoomId = 1, Room = Room1, SinceReservation = new DateTime(2019, 8, 1, 12, 47, 0), UntilReservation = new DateTime(2019, 8, 1, 12, 50, 0), Recruiter = consultant1 };
            var Reservation3 = new Reservation { Description = "Comunidad de Recursos Humanos", RoomId = 2, Room = Room2, SinceReservation = new DateTime(2019, 8, 1, 10, 47, 0), UntilReservation = new DateTime(2019, 8, 1, 10, 50, 0), Recruiter = consultant1 };
            var Reservation4 = new Reservation { Description = "Comunidad de Marketing", RoomId = 3, Room = Room3, SinceReservation = new DateTime(2019, 8, 5, 10, 47, 0), UntilReservation = new DateTime(2019, 8, 5, 10, 50, 0), Recruiter = consultant1 };
            context.Reservation.Add(Reservation1);
            context.Reservation.Add(Reservation2);
            context.Reservation.Add(Reservation3);
            context.Reservation.Add(Reservation4);
            context.SaveChanges();
            #endregion

            #region DeclineReason
            var declineReason1 = new DeclineReason { Name = "Salary", Description = "Didn't like the offered figure." };
            var declineReason2 = new DeclineReason { Name = "Different Job", Description = "Took an offer from another employer." };
            var declineReason3 = new DeclineReason { Name = "Position", Description = "Didn't like the offered position." };
            var declineReason4 = new DeclineReason { Name = "Other", Description = "Was on vacation and couldn't join." };
            var declineReason5 = new DeclineReason { Name = "Other", Description = "Disagreed with the political views of the company." };
            context.DeclineReasons.Add(declineReason1);
            context.DeclineReasons.Add(declineReason2);
            context.DeclineReasons.Add(declineReason3);
            context.DeclineReasons.Add(declineReason4);
            context.DeclineReasons.Add(declineReason5);
            context.SaveChanges();
            #endregion

        }
    }
}
