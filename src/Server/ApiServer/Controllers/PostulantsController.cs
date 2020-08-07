// <copyright file="PostulantsController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Postulant;
    using AutoMapper;
    using Core;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class PostulantsController : BaseController<PostulantsController>
    {
        private readonly IPostulantService postulantService;
        private readonly IMapper mapper;

        public PostulantsController(
            IPostulantService postulantService, ILog<PostulantsController> logger, IMapper mapper) : base(logger)
        {
            this.postulantService = postulantService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var postulants = this.postulantService.List();
                return this.Accepted(this.mapper.Map<List<ReadedPostulantViewModel>>(postulants));
            });
        }
    }
}
