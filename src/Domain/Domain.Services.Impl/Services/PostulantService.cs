// <copyright file="PostulantService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Contracts.Postulant;
    using Domain.Services.Interfaces.Services;

    public class PostulantService : IPostulantService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Postulant> postulantRepository;

        public PostulantService(
            IMapper mapper,
            IRepository<Postulant> postulandRepository)
        {
            this.mapper = mapper;
            this.postulantRepository = postulandRepository;
        }

        public ReadedPostulantContract Read(int id)
        {
            var postulantQuery = this.postulantRepository
                .QueryEager()
                .Where(_ => _.Id == id);
            var postulantResult = postulantQuery.SingleOrDefault();
            return this.mapper.Map<ReadedPostulantContract>(postulantResult);
        }

        public IEnumerable<ReadedPostulantContract> List()
        {
            var postulantQuery = this.postulantRepository
                .QueryEager();
            var postulantResult = postulantQuery.ToList();
            return this.mapper.Map<List<ReadedPostulantContract>>(postulantResult);
        }
    }
}
