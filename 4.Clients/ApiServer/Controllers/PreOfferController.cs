using ApiServer.Contracts.PreOffer;
using AutoMapper;
using Core;
using Domain.Services.Contracts.PreOffer;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

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

        /// <summary>
        /// Get All PreOffer.
        /// </summary>
        /// <response code="202">return a list of ReadedPreOfferViewModel.</response>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var preOffer = _preOfferService.List();

                return Accepted(_mapper.Map<List<ReadedPreOfferViewModel>>(preOffer));
            });
        }

        /// <summary>
        /// Get PreOffer by Id.
        /// </summary>
        /// <param name="id">preOfferId.</param>
        /// <response code="202">return a  ReadedPreOfferViewModel.</response>
        /// <response code="404">if preoffer is null.</response>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
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

        /// <summary>
        /// Add new preOffer.
        /// </summary>
        /// <param name="createPreOfferViewModel">createPreOfferViewModel.</param>
        /// <response code="202">return a ReadedPreOfferViewModel.</response>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public IActionResult Post([FromBody] CreatePreOfferViewModel createPreOfferViewModel)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreatePreOfferContract>(createPreOfferViewModel);
                var returnContract = _preOfferService.Create(contract);

                return Created("Get", _mapper.Map<CreatedPreOfferViewModel>(returnContract));
            });
        }

        /// <summary>
        /// Update a PreOffer model by Id.
        /// </summary>
        /// <param name="id">preOfferId.</param>
        /// <param name="updatePreOfferViewModel">updatePreOfferViewModel.</param>
        /// <response code="202">return an Id PreOffer.</response>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public IActionResult Put(int id, [FromBody]UpdatePreOfferViewModel updatePreOfferViewModel)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdatePreOfferContract>(updatePreOfferViewModel);
                contract.Id = id;
                _preOfferService.Update(contract);

                return Accepted(new { id });
            });
        }

        /// <summary>
        /// Delete a PreOffer by Id.
        /// </summary>
        /// <param name="id">preOfferId.</param>
        /// <response code="202">return null.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
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
