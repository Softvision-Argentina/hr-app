using ApiServer.Contracts;
using AutoMapper;
using Domain.Services.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Profiles
{
    public class DeclineReasonProfile : Profile
    {
        public DeclineReasonProfile()
        {
            CreateMap<CreateDeclineReasonViewModel, CreateDeclineReasonContract>();
            CreateMap<CreatedDeclineReasonContract, CreatedDeclineReasonViewModel>();
            CreateMap<ReadedDeclineReasonContract, ReadedDeclineReasonViewModel>();
            CreateMap<UpdateDeclineReasonViewModel, UpdateDeclineReasonContract>();
        }

        private static string ConverToString(System.Type propertyType, object propertyValue)
        {
            var enumProperty = propertyValue as IEnumerable;
            if (enumProperty != null && propertyType != typeof(string))
            {
                var stringValues = new List<string>();
                foreach (var item in enumProperty)
                {
                    stringValues.Add(item.ToString());
                }
                return string.Join(",", stringValues);
            }
            return propertyValue.ToString();
        }
    }
}
