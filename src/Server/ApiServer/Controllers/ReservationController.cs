// <copyright file="ReservationController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Reservation;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Reservation;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : BaseController<ReservationController>
    {
        private readonly IReservationService reservationService;
        private readonly IMapper mapper;

        public ReservationController(
            IReservationService reservationService,
            ILog<ReservationController> logger,
            IMapper mapper) : base(logger)
        {
            this.reservationService = reservationService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var communities = this.reservationService.List();

                return this.Accepted(this.mapper.Map<List<ReadedReservationViewModel>>(communities));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var reservation = this.reservationService.Read(id);

                if (reservation == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedReservationViewModel>(reservation));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateReservationViewModel createReservationVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateReservationContract>(createReservationVm);
                var returnContract = this.reservationService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedReservationViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateReservationViewModel updateReservationVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateReservationContract>(updateReservationVm);
                contract.Id = id;
                this.reservationService.Update(contract);

                return this.Accepted();
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.reservationService.Delete(id);
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