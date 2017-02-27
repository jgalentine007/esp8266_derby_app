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

            Den den = new Den();
            den.name = "terrible tigers";
            den.rank = "tiger";

            derby.dens.Add(den);

            Car car = new Car();
            car.name = "joe";
            car.weight = 1.0;
            car.number = 1;
            car.denID = den.ID;

            derby.AddCar(car.ID, car.name, car.weight, den.ID, car.number);

            Assert.Contains(car, derby.cars);
            Assert.Contains(car.ID, derby.dens.Find(a => a.ID == den.ID).carIDs);
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

        [Test()]
        public void DeleteCarTest()
        {
            Derby derby = new Derby();

            Car car = new Car();
            car.name = "joe";
            car.weight = 1.0;
            car.number = 1;

            derby.cars.Add(car);

            derby.DeleteCar(car.ID);

            Assert.AreEqual(derby.cars.Count, 0);
        }

        [Test()]
        public void GetCarTest()
        {
            Derby derby = new Derby();

            Car car = new Car();
            car.name = "joe";
            car.weight = 1.0;
            car.number = 1;

            derby.cars.Add(car);

            Car newCar = derby.GetCar(car.ID);

            Assert.AreEqual(car, newCar);
        }

        [Test()]
        public void NewRaceTest()
        {
            Derby derby = new Derby();

            Guid denID = derby.AddDen("terrible tigers", "tiger");
            derby.AddCar(Guid.NewGuid(), "joe", 1.0, denID, 1);
            derby.AddCar(Guid.NewGuid(), "bob", 1.0, denID, 2);
            derby.AddCar(Guid.NewGuid(), "tim", 1.0, denID, 3);
            derby.AddCar(Guid.NewGuid(), "mac", 1.0, denID, 3);

            derby.NewRace();

            Assert.AreEqual(derby.participants.Count, derby.trackLanes);
        }

        [Test()]
        public void PositionCarsTest()
        {
            Derby derby = new Derby();

            Guid denID = derby.AddDen("terrible tigers", "tiger");
            derby.AddCar(Guid.NewGuid(), "joe", 1.0, denID, 1);
            derby.AddCar(Guid.NewGuid(), "bob", 1.0, denID, 2);
            derby.AddCar(Guid.NewGuid(), "tim", 1.0, denID, 3);
            derby.AddCar(Guid.NewGuid(), "mac", 1.0, denID, 3);

            derby.PositionCars();

            foreach (Car car in derby.cars)
            {
                foreach (List<Guid> mylst in derby.laneSchedule)
                {
                    Assert.AreEqual(mylst.FindAll(a => a == car.ID).Count(), 1);
                }
            }

        }

        [Test()]
        public void DeleteRaceTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void FinishRaceTest()
        {
            Assert.Fail();
        }
    }
}