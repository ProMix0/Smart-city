using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Building : MonoBehaviour
    {
        internal GameObject model;
        internal City cityParent;

        public Building(string path, City city)
        {
            cityParent = city;
            model = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
        }
        public void SetCoordinates(int x, int y, int z)
        {
            model.transform.position = new Vector3(x, y, z);
        }
    }

    public class Home : Building
    {
        public List<Citizen> Citizens { get; } = new List<Citizen>();
        public Dictionary<string, bool> Satisfaction { get; } = new Dictionary<string, bool>
        { { "Shop", false },{"ScienceCenter",false },{ "Park", false },{"CarPark",false } };
        public Home(string path, City city) : base(path, city)
        {
            for (int i = 0; i < 3 && city.FreeCitizens.Count > 0; i++)
                Citizens.Add(city.FreeCitizens.Dequeue());
        }
        public void AddCitizen(Citizen citizen)
        {
            foreach (var item in Satisfaction)
                citizen.Satisfaction[item.Key] = item.Value;
        }
        public void RemoveCitizen(Citizen citizen)
        {
            foreach (var item in citizen.Satisfaction)
                citizen.Satisfaction[item.Key] = false;
        }
    }

    public class SatisfactionBuilding : Building
    {
        public int Radius { get; internal set; }
        public int GridX { get; set; }
        public int GridY { get; set; }
        internal string satisfactionName;
        public SatisfactionBuilding(string path, City city) : base(path, city)
        {
        }

        public void UpdateSatisfaction()
        {
            for (int i = Mathf.Max(0, GridX - Radius); i < Mathf.Min(GridX + Radius + 1, cityParent.Grid.GetLength(0)); i++)
                for (int j = Mathf.Max(0, GridY - Radius); j < Mathf.Min(GridY + Radius + 1, cityParent.Grid.GetLength(1)); j++)
                    if (cityParent.Grid[i, j] != null && cityParent.Grid[i, j] is Home home) home.Satisfaction[satisfactionName] = true;
        }
    }
    public class Shop : SatisfactionBuilding
    {
        public Shop(City city) : base("Shop", city)
        {
            satisfactionName = "Shop";
            Radius = 5;
        }
    }
    public class ScienceCenter : SatisfactionBuilding
    {
        public ScienceCenter(City city) : base("ScienceCenter", city)
        {
            satisfactionName = "ScienceCenter";
            Radius = 16;
        }
    }
    public class Park : SatisfactionBuilding
    {
        public Park(City city) : base("Park", city)
        {
            satisfactionName = "Park";
            Radius = 13;
        }
    }
    public class CarPark : SatisfactionBuilding
    {
        public CarPark(City city) : base("CarPark", city)
        {
            satisfactionName = "CarPark";
            Radius = 20;
        }
    }
}