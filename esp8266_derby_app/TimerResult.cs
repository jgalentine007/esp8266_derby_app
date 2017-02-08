using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace esp8266_derby_app
{
    public class TimerResult
    {
        [JsonProperty("LaneTimes")]
        public List<double> LaneTimes { get; set; }

        [JsonProperty("RaceStarted")]
        public bool RaceStarted { get; set; }
    }
}
