using Microsoft.EntityFrameworkCore;

namespace ShellTemperature.Data
{
    /// <summary>
    /// A class representinng the ladle shell database
    /// </summary>
    public class ShellDb : DbContext
    {
        #region Constructors
        public ShellDb()
        {

        }

        public ShellDb(DbContextOptions<ShellDb> dbContext) : base(dbContext)
        {

        }
        #endregion

        #region Tables
        public DbSet<ShellTemp> ShellTemperatures { get; set; }
        public DbSet<DeviceInfo> DevicesInfo { get; set; }
        #endregion
    }
}