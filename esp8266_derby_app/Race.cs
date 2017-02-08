using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esp8266_derby_app
{
    public class Race
    {
        public DateTime dateTime { get; set; }
        public int number { get; set; }
        public List<Guid> finishIDs { get; set; } = new List<Guid>();
        public Guid ID { get; set; } = Guid.NewGuid();
        public string DisplayMember { get { return number + "-" + dateTime.ToString(); } }
    }
}
