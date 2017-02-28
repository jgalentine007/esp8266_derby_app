using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esp8266_derby_app
{
    public interface ITimer
    {
        bool Test();
        bool NewRace();
        bool Results(out TimerResult timerResult);
    }
}
