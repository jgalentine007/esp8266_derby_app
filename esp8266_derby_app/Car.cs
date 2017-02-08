using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esp8266_derby_app
{
    public class Car
    {
        public string name { get; set; } = "";
        public int number { get; set; } = 0;
        public double weight { get; set; } = 0.0;
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid denID { get; set; }
        public List<Guid>  finishIDs { get; set; } = new List<Guid>();
        public string DisplayMember { get { return name + " #" + number; } }       
    }
}
