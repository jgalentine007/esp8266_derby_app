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
        private Random random = new Random();

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
                timerResult.LaneTimes.Add(GetRandomNumber(1.0,10.0));
            }

            return true;
        }

        public bool Test()
        {
            return true;
        }

        private double GetRandomNumber(double minimum, double maximum)
        {            
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
