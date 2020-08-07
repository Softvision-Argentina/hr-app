// <copyright file="ReaddressReasonController.cs" company="Softvision">
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
    public class ReaddressReasonController : BaseController<ReaddressReasonController>
    {
        private readonly IReaddressReasonService readdressReasonService;

        public ReaddressReasonController(
            IReaddressReasonService readdressReasonService,
            ILog<ReaddressReasonController> logger) : base(logger)
        {
            this.readdressReasonService = readdressReasonService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var reAddressList = this.readdressReasonService.List();
                return this.Accepted(reAddressList);
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var readedReaddressReason = this.readdressReasonService.Read(id);

                if (readedReaddressReason == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(readedReaddressReason);
            });
        }

        [HttpPost("Filter")]
        public IActionResult Filter([FromBody] ReaddressReasonSearchModel readdressReasonSearchModel)
        {
            return this.ApiAction(() =>
            {
                var readedReaddressReason = this.readdressReasonService.ListBy(readdressReasonSearchModel);
                return this.Accepted(readedReaddressReason);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateReaddressReason createReaddressReason)
        {
            return this.ApiAction(() =>
            {
                var createdReaddressReason = this.readdressReasonService.Create(createReaddressReason);
                return this.Created("/Post", createdReaddressReason);
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateReaddressReason updateReaddressReason)
        {
            return this.ApiAction(() =>
            {
                this.readdressReasonService.Update(id, updateReaddressReason);
                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.readdressReasonService.Delete(id);
                return this.Accepted();
            });
        }
    }
}