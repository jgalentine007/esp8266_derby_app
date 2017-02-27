using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace esp8266_derby_app
{
    public class Esp8266Timer:ITimer
    {
        string timerIPAddr { get; set; }

        public Esp8266Timer(string timerIPAddr)
        {
            this.timerIPAddr = timerIPAddr;
        }

        public bool Test()
        {
            try
            {
                string result = "";
                using (WebClient wc = new WebClient())
                {
                    result = wc.DownloadString("http://" + timerIPAddr);
                    if (result != "")
                        return true;
                    else
                        return false;
                }
            } catch
            {
                return false;
            }
        }
        public bool NewRace()
        {
            try
            {
                string result = "";
                using (WebClient wc = new WebClient())
                {
                    result = wc.DownloadString("http://" + timerIPAddr + "/api/NewRace");
                }
                if (result == "")
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Results(ref TimerResult timerResult)
        {
            try
            {
                string result = "";
                using (WebClient wc = new WebClient())
                {
                    result = wc.DownloadString("http://" + timerIPAddr + "/api/Results");
                }
                if (result != "")
                {
                    timerResult = JsonConvert.DeserializeObject<TimerResult>(result);
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }

        }
    }
}
