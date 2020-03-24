﻿using Microsoft.AspNetCore.Mvc;
using ShellTemperature.Data;
using ShellTemperature.Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace ShellTemperatureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShellTemperatureController : ControllerBase
    {
        #region Fields

        private readonly IRepository<ShellTemp> _shellTempRepository;
        #endregion

        #region Constructors

        public ShellTemperatureController(IRepository<ShellTemp> shellTempRepository)
        {
            _shellTempRepository = shellTempRepository;
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<IActionResult> Create(ShellTemp shellTemp)
        {
            try
            {
                bool result = _shellTempRepository.Create(shellTemp);

                if (!result)
                    return BadRequest();

                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest();
            }
        }
        #endregion
    }
}