using ApiServer.Contracts.User;
using AutoMapper;
using Domain.Services.Contracts.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserViewModel, CreateUserContract>();
            CreateMap<CreatedUserContract, CreatedUserViewModel>();
            CreateMap<ReadedUserContract, ReadedUserViewModel>();
            CreateMap<UpdateUserViewModel, UpdateUserContract>();
            CreateMap<ReadedUserRoleContract, ReadedUserRoleViewModel>();
            CreateMap<ReadedUserViewModel,ReadedUserContract>();
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