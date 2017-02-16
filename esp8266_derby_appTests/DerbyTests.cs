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

        [Test()]
        public void AddCarTest()
        {            
            Derby derby = new Derby();

            Car car = new Car();
            car.name = "joe";
            car.weight = 1.0;
            car.number = 1;

            derby.AddCar(car.ID, car.name, car.weight, car.ID, car.number);

            Assert.Contains(car, derby.cars);
        }

        [Test()]
        public void AddCarAlreadyExistsTest()
        {
            Derby derby = new Derby();

            Car car = new Car();
            car.name = "joe";
            car.weight = 1.0;
            car.number = 1;

            derby.AddCar(car.ID, car.name, car.weight, car.ID, car.number);
            derby.AddCar(car.ID, car.name, car.weight, car.ID, car.number);

            Assert.Contains(car, derby.cars);
            Assert.AreEqual(derby.cars.Count, 1);
        }
    }
}