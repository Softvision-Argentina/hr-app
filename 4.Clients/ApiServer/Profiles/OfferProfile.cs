using ApiServer.Contracts;
using ApiServer.Contracts.Offer;
using AutoMapper;
using Domain.Services.Contracts;
using Domain.Services.Contracts.Offer;
using System.Collections;
using System.Collections.Generic;

namespace ApiServer.Profiles
{
    public class OfferProfile : Profile
    {
        public OfferProfile()
        {
            CreateMap<CreateOfferViewModel, CreateOfferContract>();
            CreateMap<CreatedOfferContract, CreatedOfferViewModel>();
            CreateMap<ReadedOfferContract, ReadedOfferViewModel>();
            CreateMap<UpdateOfferViewModel, UpdateOfferContract>();
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
