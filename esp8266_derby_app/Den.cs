using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esp8266_derby_app
{
    public class Den
    {        
        public string rank { get; set; } = "";

        public string name { get; set; } = "";

        public List<Guid> carIDs { get; set; } = new List<Guid>();

        public Guid ID { get; set; } = Guid.NewGuid();

        public string DisplayMember { get { return rank + " - " + name; } }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Den that = obj as Den;
            if (this.name == that.name && this.rank == that.rank) 
                return true;
            else
                return false;
        }
    }
}
