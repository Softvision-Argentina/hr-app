using System;
using System.Collections.Generic;
using ApiServer.Contracts.SkillType;
using AutoMapper;
using Core;
using Domain.Model.Enum;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    /// <summary>
    /// Controller pensado para recuperar valores de configuración, enumeraciones, valores de un json, etc, 
    /// y disponibilizarlos en el FE. Usar cache
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigsController : ControllerBase
    {
        const string SKILL_TYPE_KEY = "SkillTypes";
        const string STAGE_TYPES_KEY = "StageStatusTypes";
        const string PING_KEY = "Key";
        const CacheLevel CACHE_LEVEL = CacheLevel.OneHour;
        const CacheGroup CACHE_GROUP = CacheGroup.Assignee;
        readonly IMemCache _cache;

        public ConfigsController(IMemCache cache, ILog<ConfigsController> logger)
        {
            this._cache = cache;
        }

        [HttpGet("SkillTypes")]
        public IActionResult SkillTypes()
        {
            var values = this.TryToGetFromCache<string>(SKILL_TYPE_KEY, () => Enum.GetNames(typeof(ReadedSkillTypeViewModel)));

            return Ok(values);
        }

        [HttpGet("StageStatusTypes")]
        public IActionResult StageStatusTypes()
        {
            var values = this.TryToGetFromCache<string>(STAGE_TYPES_KEY, () => Enum.GetNames(typeof(StageStatus)));

            return Ok(values);
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            var values = this.TryToGetFromCache<string>(PING_KEY, () => { return new string[] { "Value 1", "Value 2" }; });

            return Ok(values);
        }

        /// <summary>
        /// Se encarga de verificar si un objeto o lista de objetos se encuentran cacheados, dada una key
        /// </summary>
        /// <typeparam name="T">Tipo alojado en la cache</typeparam>
        /// <param name="key">Key a buscar en la cache</param>
        /// <param name="action">Funcion a invocar si los valores no se encuentran en la cache</param>
        /// <returns></returns>
        private IEnumerable<T> TryToGetFromCache<T>(string key, Func<IEnumerable<T>> action)
        {
            var found = _cache.TryGetValue(CACHE_GROUP, key, out IEnumerable<T> values);

            if (!found)
            {
                values = action();
                _cache.Set(CACHE_GROUP, key, values, new ExpirationSettings { AbsoluteExpiration = CACHE_LEVEL });
            }
            
            return values;
        } 
    }
}