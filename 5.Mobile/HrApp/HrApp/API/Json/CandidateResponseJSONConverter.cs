using Domain.Model;
using Domain.Model.Enum;
using HrApp.API.Beans;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HrApp.API.Json
{
    public class CandidateResponseJSONConverter : JsonConverter
    {
        private static JsonConverter instance;

        public static JsonConverter getInstance()
        {
            if (instance == null)
                instance = new CandidateResponseJSONConverter();
            return instance;
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CandidatesResponse);
        }

        public override bool CanRead => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            CandidatesResponse res = new CandidatesResponse();
            res.Name = (string)jObject["name"];
            res.LastName = (string)jObject["lastName"];
            res.DNI = (int)jObject["dni"];
            res.EmailAddress = (string)jObject["emailAddress"];
            res.PhoneNumber = (string)jObject["phoneNumber"];            
            res.LinkedInProfile = (string)jObject["linkedInProfile"];
            res.AdditionalInformation = (string)jObject["additionalInformation"];
            res.EnglishLevel = (EnglishLevel)(int)jObject["englishLevel"];
            res.Status = (CandidateStatus)(int)jObject["status"];
            res.ContactDay = DateTime.Parse((string)jObject["contactDay"]);
            res.User = jObject["recruiter"].ToObject<User>();
            res.PreferredOffice = jObject["preferredOffice"].ToObject<Office>();
          
            List<CandidateSkill> CandidateSkillsList = new List<CandidateSkill>();
            JArray candidates = (JArray)jObject["candidateSkills"];

            if (candidates != null) {
             
                foreach (var item in candidates)
                {
                    var singleCandidateSkill = new CandidateSkill();
                    var skill = new Skill();
                    singleCandidateSkill.Skill = skill;
                    singleCandidateSkill.CandidateId = (int)item["candidateId"];
                    singleCandidateSkill.SkillId = (int)item["skillId"];
                    singleCandidateSkill.Skill.Type= item["skill"].ToObject<SkillType>();
                    singleCandidateSkill.Rate = (int)item["rate"];
                    singleCandidateSkill.Comment = (string)item["comment"];
                    CandidateSkillsList.Add(singleCandidateSkill);
                }

            }
            res.CandidateSkills = CandidateSkillsList;

            return res;

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

    }
}