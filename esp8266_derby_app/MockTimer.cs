using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esp8266_derby_app
{
    public class MockTimer : ITimer
    {
        private int tracklanes;

        public MockTimer(int tracklanes)
        {
            this.tracklanes = tracklanes;
        }

        public bool NewRace()
        {
            return true;
        }

        public bool Results(out TimerResult timerResult)
        {
            timerResult = new TimerResult();

            for (int i = 0; i < tracklanes; i++)
            {
                timerResult.LaneTimes.Add(1.00 + i);
            }

            return true;
        }

        public bool Test()
        {
            return true;
        }
    }
}
