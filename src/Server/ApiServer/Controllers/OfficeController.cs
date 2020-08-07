// <copyright file="OfficeController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Office;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Office;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]

    public class OfficeController : BaseController<OfficeController>
    {
        private readonly IOfficeService officeService;
        private readonly IMapper mapper;

        public OfficeController(
            IOfficeService officeService,
            ILog<OfficeController> logger,
            IMapper mapper) : base(logger)
        {
            this.officeService = officeService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var offices = this.officeService.List();

                return this.Accepted(this.mapper.Map<List<ReadedOfficeViewModel>>(offices));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var office = this.officeService.Read(id);

                if (office == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedOfficeViewModel>(office));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateOfficeViewModel createOfficeVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateOfficeContract>(createOfficeVm);
                var returnContract = this.officeService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedOfficeViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateOfficeViewModel updateOfficeVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateOfficeContract>(updateOfficeVm);
                contract.Id = id;
                this.officeService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.officeService.Delete(id);
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