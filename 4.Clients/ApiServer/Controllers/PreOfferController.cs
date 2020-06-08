using ApiServer.Contracts.PreOffer;
using AutoMapper;
using Core;
using Domain.Services.Contracts.PreOffer;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreOfferController : BaseController<PreOfferController>
    {
        private readonly IPreOfferService _preOfferService;
        private readonly IMapper _mapper;

        public PreOfferController(
            IPreOfferService preOfferService, 
            ILog<PreOfferController> logger, 
            IMapper mapper) : base(logger)
        {
            _preOfferService = preOfferService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var preOffer = _preOfferService.List();

                return Accepted(_mapper.Map<List<ReadedPreOfferViewModel>>(preOffer));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var preOffer = _preOfferService.Read(id);

                if (preOffer == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedPreOfferViewModel>(preOffer));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreatePreOfferViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreatePreOfferContract>(vm);
                var returnContract = _preOfferService.Create(contract);

                return Created("Get", _mapper.Map<CreatedPreOfferViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdatePreOfferViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdatePreOfferContract>(vm);
                contract.Id = id;
                _preOfferService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _preOfferService.Delete(id);
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
