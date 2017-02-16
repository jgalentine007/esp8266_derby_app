using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esp8266_derby_app
{
    public class Derby
    {
        public Pack pack { get; set; } = new Pack();
        public List<Den> dens { get; set; } = new List<Den>();
        public List<Car> cars { get; set; } = new List<Car>();
        public List<Race> races { get; set; } = new List<Race>();
        public List<FinishTime> finishTimes { get; set; } = new List<FinishTime>();
        public List<List<Guid>> laneSchedule = new List<List<Guid>>();
        public List<Car> participants = new List<Car>();
        public bool saved { get; set; } = false;
        public string savedFileName { get; set; } = "";
        public string timerIP { get; set; } = "";
        public bool useTimer { get; set; } = false;
        public int heatsPerCar { get; set; } = 4;
        public int trackLanes { get; set; } = 4;
        public int redoRaceNumber = 0;

        public Guid AddDen(string name, string rank)
        {
            Den newDen = new Den();
            newDen.rank = rank;
            newDen.name = name;

            // remove den if it already exists (in case of editing)
            int idx = dens.FindIndex(a => a.Equals(newDen));
            if (idx != -1)
                dens.RemoveAt(idx);

            dens.Add(newDen);

            return newDen.ID;
        }

        public void DeleteDen(Guid denID)
        {
            // delete cars that belong to the den
            cars.RemoveAll(g => g.denID == denID);            

            int idx = dens.FindIndex(a => a.ID == denID);
            dens.RemoveAt(idx);
        }

        public Den GetDen(Guid denID)
        {
            int idx = dens.FindIndex(a => a.ID == denID);
            Den newDen = dens.ElementAt(idx);
            return newDen;
        }

        public void AddCar(Guid ID, string name, double weight, Guid denID, int number)
        {
            Car newCar = new Car();
            newCar.name = name;
            newCar.weight = weight;
            newCar.denID = denID;
            newCar.number = number;
            newCar.ID = ID;

            // remove car if it already exists (in case of editing)
            int idx = cars.FindIndex(a => a.ID == newCar.ID);
            if (idx != -1)
                cars.RemoveAt(idx);

            cars.Add(newCar);
            AddCarToDen(denID, newCar.ID);           
        }

        private void AddCarToDen(Guid denID, Guid carID)
        {
            int idx = dens.FindIndex(a => a.ID == denID);
            if (idx != -1)
            {
                if (!dens[idx].carIDs.Contains(carID))
                    dens[idx].carIDs.Add(carID);
            }                
        }

        public void DeleteCar(Guid carID)
        {
            int idx = cars.FindIndex(a => a.ID == carID);
            cars.RemoveAt(idx);
        }

        public Car GetCar(Guid carID)
        {
            int idx = cars.FindIndex(a => a.ID == carID);
            Car newCar = cars.ElementAt(idx);
            return newCar;
        }

        public void NewRace()
        {
            if (races.Count == 0)
            {                
                PositionCars();
            }

            participants.Clear();
            for (int i = 0; i < trackLanes; i++)
            {
                Guid participantID = laneSchedule[i][0];
                laneSchedule[i].RemoveAt(0);
                Car newParticipant = cars.Where(g => g.ID == participantID).First();
                participants.Add(newParticipant);
            }
        }

        public void DeleteRace(Guid raceID)
        {
            List<FinishTime> times = finishTimes.Where(finishTime => finishTime.raceID == raceID).OrderBy(i => i.lane).ToList();

            foreach (FinishTime finishTime in finishTimes)
            {
                // ugly but efficient way to remove finishIDs from selected car
                (cars.Where(car => car.ID == finishTime.carID).First()).finishIDs.RemoveAll(finishID => finishID == finishTime.ID);
                finishTimes.RemoveAll(i => i.ID == finishTime.ID);
            }

            races.RemoveAll(race => race.ID == raceID);
        }

        public void RedoRace(Guid raceID)
        {
            participants.Clear();

            List<FinishTime> finishTimesFiltered = finishTimes.Where(finishTime => finishTime.raceID == raceID).OrderBy(i => i.lane).ToList();

            foreach (FinishTime finishTime in finishTimesFiltered)
            {
                Car newCar = cars.Where(car => car.ID == finishTime.carID).First();
                participants.Add(newCar);
                // ugly but efficient way to remove finishIDs from selected car
                (cars.Where(car => car.ID == finishTime.carID).First()).finishIDs.RemoveAll(finishID => finishID == finishTime.ID);
                finishTimes.RemoveAll(i => i.ID == finishTime.ID);
            }
            redoRaceNumber = (races.Where(race => race.ID == raceID)).Select(i => i.number).First();
            races.RemoveAll(race => race.ID == raceID);
        }

        public bool FinishRace()
        {
            TimerResult timerResult = new TimerResult();

            if (Timer.Results(timerIP, ref timerResult))
            {                
                Race newRace = new Race();
                newRace.dateTime = DateTime.Now;

                // if no 'redo race number' has been assigned, then append to the race, otherwise assign
                if (redoRaceNumber == 0)
                    newRace.number = races.Count + 1;
                else
                {
                    newRace.number = redoRaceNumber;
                    redoRaceNumber = 0;
                }

                for (int i = 0; i < timerResult.LaneTimes.Count; i++)
                {
                    FinishTime newFinishTime = new FinishTime();
                    newFinishTime.lane = i + 1; // compensate zero based list
                    newFinishTime.carID = participants[i].ID;
                    newFinishTime.time = timerResult.LaneTimes[i];
                    newFinishTime.raceID = newRace.ID;

                    finishTimes.Add(newFinishTime);
                    newRace.finishIDs.Add(newFinishTime.ID);

                    // ugly but effective way of updating car finish IDs
                    (cars.Where(g => g.ID == participants[i].ID).First()).finishIDs.Add(newFinishTime.ID);
                }
                races.Add(newRace);
                participants.Clear();

                return true;            
            }
            else
            {                
                return false;
            }
        }

        public void PositionCars()
        {
            List<Guid> carIDs = new List<Guid>();
            foreach (Car car in cars)
            {
                carIDs.Add(car.ID);
            }

            List<List<Guid>> tempLaneParticipants = new List<List<Guid>>();

            laneSchedule.Clear();
            // randomize the list of cars and load them into a new list for the first lane
            Random rnd = new Random();
            List<Guid> lane1 = new List<Guid>(carIDs.OrderBy(g => rnd.Next()));
            tempLaneParticipants.Add(lane1);
            laneSchedule.Add(new List<Guid>());

            // create remaining track lanes
            for (int i = 0; i < trackLanes - 1; i++)
            {
                // copy the previous lane queue into a new list for the next lane
                List<Guid> newLane = new List<Guid>((List<Guid>)tempLaneParticipants[i]);
                // rotate the list once so that each car is never matched with itself
                newLane.Add(newLane[0]);
                newLane.RemoveAt(0);
                tempLaneParticipants.Add(newLane);
                laneSchedule.Add(new List<Guid>());
            }

            List<int> randomlist = new List<int>();
            for (int i = 0; i < cars.Count(); i++)
            {
                randomlist.Add(i);
            }


            randomlist = randomlist.OrderBy(g => rnd.Next()).ToList();
            foreach (int entry in randomlist)
            {
                for (int j = 0; j < trackLanes; j++)
                {
                    laneSchedule[j].Add(tempLaneParticipants[j][entry]);
                }
            }

        }
    }
}
