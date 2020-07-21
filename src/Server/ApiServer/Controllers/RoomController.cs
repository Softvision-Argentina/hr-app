using System.Collections.Generic;
using ApiServer.Contracts.Room;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Room;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RoomController : BaseController<RoomController> {

        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public RoomController(
            IRoomService roomService,
            ILog<RoomController> logger,
            IMapper mapper): base(logger)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var rooms = _roomService.List();

                return Accepted(_mapper.Map<List<ReadedRoomViewModel>>(rooms));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var room = _roomService.Read(id);

                if (room == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedRoomViewModel>(room));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateRoomViewModel createRoomVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateRoomContract>(createRoomVm);
                var returnContract = _roomService.Create(contract);

                return Created("Get", _mapper.Map<CreatedRoomViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateRoomViewModel updateRoomVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateRoomContract>(updateRoomVm);
                contract.Id = id;
                _roomService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _roomService.Delete(id);
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