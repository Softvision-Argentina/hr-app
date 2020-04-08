using AutoMapper;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Postulant;
using Domain.Services.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class PostulantService : IPostulantService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Postulant> _postulantRepository;

        public PostulantService(
            IMapper mapper,
            IRepository<Postulant> postulandRepository
            )
        {
            _mapper = mapper;
            _postulantRepository = postulandRepository;
        }
        public ReadedPostulantContract Read(int id)
        {
            var postulantQuery = _postulantRepository
                .QueryEager()
                .Where(_ => _.Id == id);
            var postulantResult = postulantQuery.SingleOrDefault();
            return _mapper.Map<ReadedPostulantContract>(postulantResult);
        }
        public IEnumerable<ReadedPostulantContract> List()
        {
            var postulantQuery = _postulantRepository
                .QueryEager();
            var postulantResult = postulantQuery.ToList();
            return _mapper.Map<List<ReadedPostulantContract>>(postulantResult);
        }
    }
}
