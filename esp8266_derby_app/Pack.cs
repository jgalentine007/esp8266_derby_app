using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esp8266_derby_app
{
    public class Pack
    {
        public string name { get; set; } = "";
        public string raceMaster { get; set; } = "";       
        public List<Guid> denIDs { get; set; } = new List<Guid>();
    }
}
