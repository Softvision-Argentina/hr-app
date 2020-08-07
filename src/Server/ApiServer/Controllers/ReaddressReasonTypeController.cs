// <copyright file="ReaddressReasonTypeController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using Core;
    using Domain.Services.Contracts.ReaddressReason;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ReaddressReasonTypeController : BaseController<ReaddressReasonTypeController>
    {
        private readonly IReaddressReasonTypeService readdressReasonTypeService;

        public ReaddressReasonTypeController(
            IReaddressReasonTypeService readdressReasonTypeService,
            ILog<ReaddressReasonTypeController> logger) : base(logger)
        {
            this.readdressReasonTypeService = readdressReasonTypeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var readdressReason = this.readdressReasonTypeService.List();
                return this.Accepted(readdressReason);
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var readdressReason = this.readdressReasonTypeService.Read(id);

                if (readdressReason == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(readdressReason);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateReaddressReasonType createdReaddressReasonType)
        {
            return this.ApiAction(() =>
            {
                var returnContract = this.readdressReasonTypeService.Create(createdReaddressReasonType);
                return this.Created("/Post", returnContract);
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateReaddressReasonType updateReaddressReasonType)
        {
            return this.ApiAction(() =>
            {
                this.readdressReasonTypeService.Update(id, updateReaddressReasonType);
                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.readdressReasonTypeService.Delete(id);
                return this.Accepted(new { id });
            });
        }
    }
}