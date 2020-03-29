using Microsoft.EntityFrameworkCore;
using ShellTemperature.Data;
using ShellTemperature.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShellTemperature.Repository
{
    public sealed class ShellTemperatureRepository : BaseRepository, IShellTemperatureRepository<ShellTemp>
    {
        #region Constructors
        public ShellTemperatureRepository(ShellDb context) : base(context) { }
        #endregion

        #region Create
        /// <summary>
        /// Create a new shell temperature in the database
        /// </summary>
        /// <param name="model">Model item to create</param>
        /// <returns>Returns true if the model was successfully created</returns>
        public async Task<bool> Create(ShellTemp model)
        {
            if (model?.Device == null)
                throw new ArgumentNullException(nameof(model), "The model supplied was invalid");

            // Try and find the device in the database
            DeviceInfo dbDevice = await Context.DevicesInfo.FindAsync(model.Device.Id) ??
                                  await Context.DevicesInfo.FirstOrDefaultAsync(dev =>
                                                dev.DeviceAddress.Equals(model.Device.DeviceAddress));

            DeviceInfo device = dbDevice ?? model.Device; // Use the database device or add models device
            model.Device = device;

            await Context.AddAsync(model);
            await Context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Get

        public async Task<ShellTemp> GetItem(Guid id)
        {
            return await Context.ShellTemperatures.FindAsync(id);
        }

        /// <summary>
        /// Get all of the shell temperature records from the database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ShellTemp>> GetAll()
        {
            return await Context.ShellTemperatures
                .Include(dev => dev.Device)
                .ToListAsync();
        }

        public async Task<IEnumerable<ShellTemp>> GetShellTemperatureData(DateTime start, DateTime end)
        {
            return await Context.ShellTemperatures
                .Include(dev => dev.Device)
                .Where(dateTime => dateTime.RecordedDateTime >= start && dateTime.RecordedDateTime <= end)
                .ToListAsync();
        }

        public async Task<IEnumerable<ShellTemp>> GetShellTemperatureData(DateTime start, DateTime end, string deviceName = null,
            string deviceAddress = null)
        {
            return await Context.ShellTemperatures
                .Include(dev => dev.Device)
                .Where(dateTime => dateTime.RecordedDateTime >= start && dateTime.RecordedDateTime <= end)
                .Where(device =>
                    string.IsNullOrWhiteSpace(deviceName) || device.Device.DeviceName.Equals(deviceName) &&
                    string.IsNullOrWhiteSpace(deviceAddress) || device.Device.DeviceAddress.Equals(deviceAddress))
                .ToListAsync();
        }

        #endregion

        #region Delete
        /// <summary>
        /// Delete a shell temperature by id
        /// </summary>
        /// <param name="id">Id of the element to delete</param>
        /// <returns>Returns true if deleted successfully</returns>
        public async Task<bool> Delete(Guid id)
        {
            ShellTemp shellTemp = await Context.ShellTemperatures.FindAsync(id);
            if (shellTemp == null)
                return false;

            Context.ShellTemperatures.Remove(shellTemp);
            await Context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Delete many shell temperatures
        /// </summary>
        /// <param name="items">Shell temperature collection to delete</param>
        /// <returns>Returns true if the elements where deleted</returns>
        public async Task<bool> DeleteRange(IEnumerable<ShellTemp> items)
        {
            if (items == null)
                return false;

            foreach (var shellTemp in items)
            {
                ShellTemp dbTemp = await Context.ShellTemperatures.FindAsync(shellTemp.Id);
                Context.ShellTemperatures.Remove(dbTemp);
            }

            await Context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update the element with new information.
        /// Find the existing element by id and update with the given models data.
        /// </summary>
        /// <param name="model">The data to update with element with</param>
        /// <returns>Returns true if updated successfully</returns>
        public async Task<bool> Update(ShellTemp model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "The shell temperature was invalid");

            ShellTemp dbShellTemp = await GetItem(model.Id);
            if (dbShellTemp == null)
                throw new NullReferenceException("Could not find the shell temperature in the database");

            DeviceInfo dbDeviceInfo = await Context.DevicesInfo.FindAsync(model.Device.Id);
            if (dbDeviceInfo == null)
                throw new NullReferenceException("Could not find the device in the database");

            dbShellTemp.Temperature = model.Temperature;
            dbShellTemp.RecordedDateTime = model.RecordedDateTime;
            dbShellTemp.Latitude = model.Latitude;
            dbShellTemp.Longitude = model.Longitude;
            dbShellTemp.Device = dbDeviceInfo;

            await Context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}