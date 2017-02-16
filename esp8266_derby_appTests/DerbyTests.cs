using NUnit.Framework;
using esp8266_derby_app;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esp8266_derby_app.Tests
{
    [TestFixture()]
    public class DerbyTests
    {
        [Test()]
        public void AddDenTest()
        {
            Den den = new Den();
            den.name = "terrible tigers";
            den.rank = "tiger";

            Derby derby = new Derby();
            derby.AddDen(den.name, den.rank);

            Assert.Contains(den, derby.dens);
        }

        [Test()]
        public void AddDenTestAlreadyExists()
        {
            Den den = new Den();
            den.name = "terrible tigers";
            den.rank = "tiger";

            Derby derby = new Derby();
            derby.AddDen(den.name, den.rank);
            derby.AddDen(den.name, den.rank);

            Assert.Contains(den, derby.dens);
            Assert.AreEqual(derby.dens.Count, 1);
        }

        [Test()]
        public void DeleteDenTest()
        {
            Den den = new Den();
            den.name = "terrible tigers";
            den.rank = "tiger";

            Derby derby = new Derby();
            derby.dens.Add(den);

            derby.DeleteDen(den.ID);

            Assert.AreEqual(derby.dens.Count, 0);
        }

        [Test()]
        public void DeleteDenContainingCarsTest()
        {
            Den den = new Den();
            den.name = "terrible tigers";
            den.rank = "tiger";

            Derby derby = new Derby();
            derby.dens.Add(den);

            Car car = new Car();
            car.denID = den.ID;
            derby.cars.Add(car);

            derby.DeleteDen(den.ID);

            Assert.AreEqual(derby.cars.Count, 0);
        }

        [Test()]
        public void GetDenTest()
        {
            Den den = new Den();
            den.name = "terrible tigers";
            den.rank = "tiger";

            Derby derby = new Derby();
            derby.dens.Add(den);

            Den newDen = derby.GetDen(den.ID);

            Assert.AreEqual(den, newDen);
        }
    }
}