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
        public async Task<bool> Create(ShellTemp model)
        {
            if (model?.Device == null)
                throw new ArgumentNullException(nameof(model), "The model supplied was invalid");

            DeviceInfo dbDevice = await Context.DevicesInfo.FindAsync(model.Device.Id);
            DeviceInfo device = dbDevice ?? model.Device;
            model.Device = device;

            await Context.AddAsync(model);
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}