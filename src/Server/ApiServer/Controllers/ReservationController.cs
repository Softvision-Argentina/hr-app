using System.Collections.Generic;
using ApiServer.Contracts.Reservation;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Reservation;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : BaseController<ReservationController>
    {
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;

        public ReservationController(
            IReservationService reservationService,
            ILog<ReservationController> logger,
            IMapper mapper) : base(logger)
        {
            _reservationService = reservationService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var communities = _reservationService.List();

                return Accepted(_mapper.Map<List<ReadedReservationViewModel>>(communities));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var reservation = _reservationService.Read(id);

                if (reservation == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedReservationViewModel>(reservation));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateReservationViewModel createReservationVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateReservationContract>(createReservationVm);
                var returnContract = _reservationService.Create(contract);

                return Created("Get", _mapper.Map<CreatedReservationViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateReservationViewModel UpdateReservationVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateReservationContract>(UpdateReservationVm);
                contract.Id = id;
                _reservationService.Update(contract);

                return Accepted();
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _reservationService.Delete(id);
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