using ShellTemperature.Data;
using System;

namespace ShellTemperature.Models
{
    public class ShellTemperatureRecord
    {
        public Guid Id { get; set; }

        public double Temperature { get; set; }

        public DateTime? RecordedDateTime { get; set; }

        public float? Latitude { get; set; }

        public float? Longitude { get; set; }

        public DeviceInfo DeviceInfo { get; set; }
    }
}