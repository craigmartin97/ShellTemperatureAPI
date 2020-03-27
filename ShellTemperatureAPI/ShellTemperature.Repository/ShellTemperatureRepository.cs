using Microsoft.EntityFrameworkCore;
using ShellTemperature.Data;
using ShellTemperature.Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace ShellTemperature.Repository
{
    public sealed class ShellTemperatureRepository : BaseRepository, IRepository<ShellTemp>
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


        /// <summary>
        /// Get all of the shell temperature records from the database
        /// </summary>
        /// <returns></returns>
        public async Task<ShellTemp[]> GetAll()
            => await Context.ShellTemperatures
                .Include(dev => dev.Device)
                .ToArrayAsync();
        #endregion
    }
}