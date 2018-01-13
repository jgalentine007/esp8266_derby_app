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
        bool simulateFailure;

        public MockTimer(int tracklanes, bool simulateFailure = false)
        {
            this.tracklanes = tracklanes;
            this.simulateFailure = simulateFailure;
        }

        public bool NewRace()
        {
            if (simulateFailure)
                return false;
            else
                return true;
        }

        public bool EndRace()
        {
            if (simulateFailure)
                return false;
            else
                return true;
        }

        public bool ReadyRace()
        {
            if (simulateFailure)
                return false;
            else
                return true;
        }

        public bool Results(out TimerResult timerResult)
        {
            timerResult = new TimerResult();

            if (simulateFailure)
                return false;
            else
            {
                for (int i = 0; i < tracklanes; i++)
                {
                    timerResult.LaneTimes.Add(GetRandomNumber(1.0, 10.0));
                }

                return true;
            }
        }

        public bool Test()
        {
            if (simulateFailure)
                return false;
            else
                return true;
        }

        private double GetRandomNumber(double minimum, double maximum)
        {            
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
