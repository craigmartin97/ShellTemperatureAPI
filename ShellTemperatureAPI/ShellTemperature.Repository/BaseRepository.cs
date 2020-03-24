using ShellTemperature.Data;

namespace ShellTemperature.Repository
{
    public class BaseRepository
    {
        #region Fields
        /// <summary>
        /// Database context to access database with
        /// </summary>
        protected readonly ShellDb Context;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected internal BaseRepository(ShellDb context)
        {
            Context = context;
        }
        #endregion
    }
}