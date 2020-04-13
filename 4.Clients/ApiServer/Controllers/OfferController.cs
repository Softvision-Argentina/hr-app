using ApiServer.Contracts.Offer;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Offer;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : BaseController<OfferController>
    {
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;

        public OfferController(
            IOfferService offerService, 
            ILog<OfferController> logger, 
            IMapper mapper) : base(logger)
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
