﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShellTemperature.Repository.Interfaces
{
    public interface IShellTemperatureRepository<T> : IRepository<T>
    {
        Task<IEnumerable<T>> GetShellTemperatureData(DateTime start, DateTime end);

        Task<IEnumerable<T>> GetShellTemperatureData(DateTime start, DateTime end, string deviceName = null, string deviceAddress = null);
    }
}