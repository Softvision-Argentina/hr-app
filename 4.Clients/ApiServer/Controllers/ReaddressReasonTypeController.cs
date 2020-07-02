using Core;
using Domain.Services.Contracts.ReaddressReason;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReaddressReasonTypeController : BaseController<ReaddressReasonTypeController>
    {
        private readonly IReaddressReasonTypeService _readdressReasonTypeService;

        public ReaddressReasonTypeController(
            IReaddressReasonTypeService readdressReasonTypeService,
            ILog<ReaddressReasonTypeController> logger) : base(logger)
        {
            _readdressReasonTypeService = readdressReasonTypeService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var readdressReason = _readdressReasonTypeService.List();
                return Accepted(readdressReason);
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var readdressReason = _readdressReasonTypeService.Read(id);

                if (readdressReason == null)
                {
                    return NotFound(id);
                }

                return Accepted(readdressReason);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateReaddressReasonType createdReaddressReasonType)
        {
            return ApiAction(() =>
            {
                var returnContract = _readdressReasonTypeService.Create(createdReaddressReasonType);
                return Created("/Post", returnContract);
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateReaddressReasonType updateReaddressReasonType)
        {
            return ApiAction(() =>
            {
                _readdressReasonTypeService.Update(id, updateReaddressReasonType);
                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _readdressReasonTypeService.Delete(id);
                return Accepted(new { id });
            });
        }
    }
}