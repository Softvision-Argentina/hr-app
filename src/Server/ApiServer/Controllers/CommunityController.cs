// <copyright file="CommunityController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Community;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Community;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : BaseController<CommunityController>
    {
        private readonly ICommunityService communityService;
        private readonly IMapper mapper;

        public CommunityController(
            ICommunityService communityService,
            ILog<CommunityController> logger,
            IMapper mapper) : base(logger)
        {
            this.communityService = communityService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var communities = this.communityService.List();

                return this.Accepted(this.mapper.Map<List<ReadedCommunityViewModel>>(communities));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var community = this.communityService.Read(id);

                if (community == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedCommunityViewModel>(community));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateCommunityViewModel createCommunityVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateCommunityContract>(createCommunityVm);
                var returnContract = this.communityService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedCommunityViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateCommunityViewModel updateCommunityVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateCommunityContract>(updateCommunityVm);
                contract.Id = id;
                this.communityService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.communityService.Delete(id);
                return this.Accepted();
            });
        }

        // TODO: esto es un health check? no podemos tenerlo en un solo controller.es necesario que este en todos?
        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return this.Ok(new { Status = "OK" });
        }
    }
}