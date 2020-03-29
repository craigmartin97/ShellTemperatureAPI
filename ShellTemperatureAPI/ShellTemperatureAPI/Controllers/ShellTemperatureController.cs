using Microsoft.AspNetCore.Mvc;
using ShellTemperature.Data;
using ShellTemperature.Models;
using ShellTemperature.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShellTemperatureAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ShellTemperatureController : ControllerBase
    {
        #region Fields
        /// <summary>
        /// Shell temperature repository to engage with the shell temperature table of the database
        /// </summary>
        private readonly IShellTemperatureRepository<ShellTemp> _shellTempRepository;
        #endregion

        #region Constructors

        public ShellTemperatureController(IShellTemperatureRepository<ShellTemp> shellTempRepository)
        {
            _shellTempRepository = shellTempRepository;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create a new shell temperature record in the database
        /// </summary>
        /// <param name="record">The new record to create</param>
        /// <returns>Returns a OK if the request was a success</returns>
        [HttpPost("api/[controller]")]
        public async Task<IActionResult> Create(ShellTemperatureRecord record)
        {
            try
            {
                if (!record.RecordedDateTime.HasValue)
                    record.RecordedDateTime = DateTime.Now;

                ShellTemp shellTemp = new ShellTemp(record.Id, record.Temperature, (DateTime)record.RecordedDateTime, record.Latitude, record.Longitude, record.DeviceInfo);
                bool result = await _shellTempRepository.Create(shellTemp);

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

        #region Get
        /// <summary>
        /// Get all of the shell temperature readings from the database
        /// </summary>
        /// <returns>Returns a collection of shell temperature readings from the database</returns>
        [HttpGet("api/[controller]")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<ShellTemp> allDataReadings = await _shellTempRepository.GetAll();
            ShellTemp[] dataReadings = allDataReadings as ShellTemp[] ?? allDataReadings.ToArray();

            if (dataReadings.Length == 0)
                return BadRequest("No data could be found");

            return Ok(allDataReadings);
        }

        /// <summary>
        /// Get single shell temperature by id
        /// </summary>
        /// <param name="id">Id of the shell temperature to find</param>
        /// <returns></returns>
        [HttpGet("api/[controller]/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            ShellTemp shellTemp = await _shellTempRepository.GetItem(id);
            if (shellTemp == null)
                return BadRequest("Could not find the shell temperature item");

            return Ok(shellTemp);
        }

        /// <summary>
        /// Get shell temperature readings between two dates.
        /// Additional information about the device can be supplied
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceAddress"></param>
        /// <returns></returns>
        [HttpGet("api/[controller]/GetBetweenDates")]
        public async Task<IActionResult> Get([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string deviceName = null, [FromQuery] string deviceAddress = null)
        {
            IEnumerable<ShellTemp> shellTemps = await _shellTempRepository.GetShellTemperatureData(start, end, deviceName, deviceAddress);
            ShellTemp[] temps = shellTemps as ShellTemp[] ?? shellTemps.ToArray();

            return Ok(temps);
        }
        #endregion

        #region Delete
        [HttpDelete("api/ShellTemperature/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool deleted = await _shellTempRepository.Delete(id);
            if (!deleted)
                return BadRequest("Could not delete shell temperature");

            return Ok();
        }

        [HttpPost("api/[controller]/DeleteRange")]
        public async Task<IActionResult> Delete(IEnumerable<ShellTemp> shellTemps)
        {
            bool deleted = await _shellTempRepository.DeleteRange(shellTemps);
            if (!deleted)
                return BadRequest("Failed to delete shell temperatures");

            return Ok();
        }
        #endregion

        #region Update
        [HttpPut("api/[controller]")]
        public async Task<IActionResult> Update(ShellTemp shellTemp)
        {
            bool updated = await _shellTempRepository.Update(shellTemp);
            if (!updated)
                return BadRequest("Could not update the shell temperature");

            return Ok();
        }
        #endregion
    }
}