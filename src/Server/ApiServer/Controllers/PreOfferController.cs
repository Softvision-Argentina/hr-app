// <copyright file="PreOfferController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using ApiServer.Contracts.PreOffer;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.PreOffer;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class PreOfferController : BaseController<PreOfferController>
    {
        private readonly IPreOfferService preOfferService;
        private readonly IMapper mapper;

        public PreOfferController(
            IPreOfferService preOfferService,
            ILog<PreOfferController> logger,
            IMapper mapper) : base(logger)
        {
            this.preOfferService = preOfferService;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var preOffer = this.preOfferService.List();

                return this.Accepted(this.mapper.Map<List<ReadedPreOfferViewModel>>(preOffer));
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var preOffer = this.preOfferService.Read(id);

                if (preOffer == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedPreOfferViewModel>(preOffer));
            });
        }

        /// <summary>
        /// Get PreOffers by process Id.
        /// </summary>
        /// <param name="id">processId.</param>
        /// <response code="202">return a  ReadedPreOfferViewModel.</response>
        /// <response code="404">if preoffer is null.</response>
        /// <returns> a ReadedPreOfferViewModel obj.</returns>
        [HttpGet("GetByProcess/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public IActionResult GetByProcessId(int id)
        {
            return this.ApiAction(() =>
            {
                var preOffer = this.preOfferService.GetByProcessId(id);

                if (preOffer == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<List<ReadedPreOfferViewModel>>(preOffer));
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
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreatePreOfferContract>(createPreOfferViewModel);
                var returnContract = this.preOfferService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedPreOfferViewModel>(returnContract));
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
        public IActionResult Put(int id, [FromBody] UpdatePreOfferViewModel updatePreOfferViewModel)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdatePreOfferContract>(updatePreOfferViewModel);
                contract.Id = id;
                this.preOfferService.Update(contract);

                return this.Accepted(new { id });
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
            return this.ApiAction(() =>
            {
                this.preOfferService.Delete(id);
                return this.Accepted();
            });
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return this.Ok(new { Status = "OK" });
        }
    }
}
