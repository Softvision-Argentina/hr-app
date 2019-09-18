﻿using ApiServer.Contracts.DaysOff;
using AutoMapper;
using Domain.Services.Contracts.DaysOff;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Profiles
{
    public class DaysOffProfile : Profile
    {
        public DaysOffProfile()
        {
            CreateMap<CreateDaysOffViewModel, CreateDaysOffContract>();
            CreateMap<CreatedDaysOffContract, CreatedDaysOffViewModel>();
            CreateMap<ReadedDaysOffContract, ReadedDaysOffViewModel>();
            CreateMap<UpdateDaysOffViewModel, UpdateDaysOffContract>();
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