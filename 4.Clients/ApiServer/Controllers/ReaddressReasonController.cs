using Core;
using Domain.Services.Contracts.ReaddressReason;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReaddressReasonController : BaseController<ReaddressReasonController>
    {
        private readonly IReaddressReasonService _readdressReasonService;

        public ReaddressReasonController(
            IReaddressReasonService readdressReasonService,
            ILog<ReaddressReasonController> logger) : base(logger)
        {
            _readdressReasonService = readdressReasonService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var reAddressList = _readdressReasonService.List();
                return Accepted(reAddressList);
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var readedReaddressReason = _readdressReasonService.Read(id);

                if (readedReaddressReason == null)
                {
                    return NotFound(id);
                }

                return Accepted(readedReaddressReason);
            });
        }

        [HttpPost("Filter")]
        public IActionResult Filter([FromBody] ReaddressReasonSearchModel readdressReasonSearchModel)
        {
            return ApiAction(() =>
            {
                var readedReaddressReason = _readdressReasonService.ListBy(readdressReasonSearchModel);
                return Accepted(readedReaddressReason);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateReaddressReason createReaddressReason)
        {
            return ApiAction(() =>
            {
                var createdReaddressReason = _readdressReasonService.Create(createReaddressReason);
                return Created("/Post", createdReaddressReason);
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateReaddressReason updateReaddressReason)
        {
            return ApiAction(() =>
            {
                _readdressReasonService.Update(id, updateReaddressReason);
                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _readdressReasonService.Delete(id);
                return Accepted();
            });
        }
    }
}