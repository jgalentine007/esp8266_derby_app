using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esp8266_derby_app
{
    public class FinishTime
    {
        public double time { get; set; } = 0.0;
        public Guid carID { get; set; }
        public Guid raceID { get; set; }
        public int lane { get; set; }
        public Guid ID { get; set; } = Guid.NewGuid();
    }
}
