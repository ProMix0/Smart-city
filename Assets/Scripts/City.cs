using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class City : MonoBehaviour
    {
        public Building[,] Grid { get; private set; } = new Building[100, 100];
        public int Radius { get;private set; }
        public List<Building> Buildings { get; } = new List<Building>();
        public List<Home> Homes { get; } = new List<Home>();
        public List<SatisfactionBuilding> SatisfactionBuildings { get; } = new List<SatisfactionBuilding>();
        public List<Citizen> Citizens { get; } = new List<Citizen>();
        public Queue<Citizen> FreeCitizens { get; } = new Queue<Citizen>();

        public int Energy { get; set; }
        public GameStage Stage { get; private set; } = GameStage.First;
        public double SatisfactionPercent
        {
            get
            {
                double current = 0;
                double max = 0;
                switch(Stage)
                {
                    case GameStage.Thrid:
                        max += 3;
                        foreach(var item in Citizens)
                        {
                            if (item.Satisfaction["ScienceCenter"]) current++;
                            if (item.Satisfaction["Park"]) current++;
                            if (item.Satisfaction["CarPark"]) current++;
                        }
                        goto case GameStage.Second;
                    case GameStage.Second:
                        max += 1;
                        foreach (var item in Citizens)
                        {
                            if (item.Satisfaction["Shop"]) current++;
                        }
                        goto case GameStage.First;
                    case GameStage.First:
                        break;
                }
                max *= Citizens.Count;
                return current == 0 ? 0 : max / current;
            }
        }
        public int Experiance { get; set; }
        public double Resources { get; set; }
        public int Points { get; private set; }

        public void OnBuildBuilding(Building building)
        {
            foreach (var item in Homes)
                foreach (var keyValuePair in item.Satisfaction)
                    item.Satisfaction[keyValuePair.Key] = false;
            foreach (var item in SatisfactionBuildings)
                item.UpdateSatisfaction();
        }

        public enum GameStage
        { First, Second, Thrid }
    }
}