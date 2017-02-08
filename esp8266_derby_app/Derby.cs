using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esp8266_derby_app
{
    public class Derby
    {
        public Pack pack { get; set; } = new Pack();
        public List<Den> dens { get; set; } = new List<Den>();
        public List<Car> cars { get; set; } = new List<Car>();
        public List<Race> races { get; set; } = new List<Race>();
        public List<FinishTime> finishTimes { get; set; } = new List<FinishTime>();
        public List<List<Guid>> laneSchedule = new List<List<Guid>>();
        public bool saved { get; set; } = false;
        public string savedFileName { get; set; } = "";
        public string timerIP { get; set; } = "";
        public bool useTimer { get; set; } = false;
        public int heatsPerCar { get; set; } = 4;
        public int trackLanes { get; set; } = 4;        
    }
}
