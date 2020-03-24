using System;
using ShellTemperature.Repository.Interfaces;
using ShellTemperature.Data;

namespace ShellTemperature.Repository
{
    public sealed class ShellTemperatureRepository : BaseRepository, IRepository<ShellTemp>
    {
        #region Fields
        #endregion

        #region Constructors

        public ShellTemperatureRepository(ShellDb context) : base(context) { }
        #endregion
        public bool Create(ShellTemp model)
        {
            if (model?.Device == null)
                throw new ArgumentNullException(nameof(model), "The model supplied was invalid");

            DeviceInfo dbDevice = Context.DevicesInfo.Find(model.Device.Id);
            DeviceInfo device = dbDevice ?? model.Device;
            model.Device = device;

            Context.Add(model);
            Context.SaveChanges();
            return true;
        }
    }
}