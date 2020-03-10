using ApiServer.Contracts.Offer;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Offer;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : BaseController<OfferController>
    {
        IOfferService _offerService;
        private IMapper _mapper;

        public OfferController(IOfferService offerService, ILog<OfferController> logger, IMapper mapper) : base(logger)
        {
            _offerService = offerService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var offer = _offerService.List();

                return Accepted(_mapper.Map<List<ReadedOfferViewModel>>(offer));
            });
        }

        // GET api/offer/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var offer = _offerService.Read(id);

                if (offer == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedOfferViewModel>(offer));
            });
        }

        // POST api/offer
        // Creation
        [HttpPost]
        public IActionResult Post([FromBody] CreateOfferViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateOfferContract>(vm);
                var returnContract = _offerService.Create(contract);

                return Created("Get", _mapper.Map<CreatedOfferViewModel>(returnContract));
            });
        }

        // PUT api/offer/5
        // Mutation
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateOfferViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateOfferContract>(vm);
                contract.Id = id;
                _offerService.Update(contract);

                return Accepted(new { id });
            });
        }

        // DELETE api/offer/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _offerService.Delete(id);
                return Accepted();
            });
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new { Status = "OK" });
        }
    }
}
