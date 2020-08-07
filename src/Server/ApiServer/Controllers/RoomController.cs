// <copyright file="RoomController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Room;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Room;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]

    public class RoomController : BaseController<RoomController>
    {
        private readonly IRoomService roomService;
        private readonly IMapper mapper;

        public RoomController(
            IRoomService roomService,
            ILog<RoomController> logger,
            IMapper mapper) : base(logger)
        {
            this.roomService = roomService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var rooms = this.roomService.List();

                return this.Accepted(this.mapper.Map<List<ReadedRoomViewModel>>(rooms));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var room = this.roomService.Read(id);

                if (room == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedRoomViewModel>(room));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateRoomViewModel createRoomVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateRoomContract>(createRoomVm);
                var returnContract = this.roomService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedRoomViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateRoomViewModel updateRoomVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateRoomContract>(updateRoomVm);
                contract.Id = id;
                this.roomService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.roomService.Delete(id);
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