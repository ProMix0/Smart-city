using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class City : MonoBehaviour
    {
        public const int GridSideSize = 100, CellSizeAsCoordinates = 50;

        public Building[,] Grid { get; private set; } = new Building[GridSideSize, GridSideSize];
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

        public void Start()
        {
            int yCoordinate = 0, xCoordinate = GridSideSize * CellSizeAsCoordinates / 2, zCoordinate = xCoordinate;
            List<Vector3> lines = new List<Vector3> { new Vector3(xCoordinate, yCoordinate, -zCoordinate),
                new Vector3(xCoordinate, yCoordinate, zCoordinate) };
            while (xCoordinate > -GridSideSize * CellSizeAsCoordinates / 2)
            {
                xCoordinate -= CellSizeAsCoordinates;
                lines.Add(new Vector3(xCoordinate, yCoordinate, zCoordinate));
                zCoordinate = -zCoordinate;
                lines.Add(new Vector3(xCoordinate, yCoordinate, zCoordinate));
            }
            xCoordinate = -xCoordinate;
            lines.Add(new Vector3(xCoordinate, yCoordinate, zCoordinate));
            while (zCoordinate > -GridSideSize * CellSizeAsCoordinates / 2)
            {
                zCoordinate -= CellSizeAsCoordinates;
                lines.Add(new Vector3(xCoordinate, yCoordinate, zCoordinate));
                xCoordinate = -xCoordinate;
                lines.Add(new Vector3(xCoordinate, yCoordinate, zCoordinate));
            }
            GameObject visualGrid = new GameObject();
            LineRenderer lineRenderer= visualGrid.AddComponent<LineRenderer>();
            lineRenderer.positionCount = lines.Count;
            lineRenderer.SetPositions(lines.ToArray());

        }

        public void OnBuildBuilding(Building building)
        {
            foreach (var item in Homes)
                foreach (var keyValuePair in item.Satisfaction)
                    item.Satisfaction[keyValuePair.Key] = false;
            foreach (var item in SatisfactionBuildings)
                item.UpdateSatisfaction();
        }

        public static Tuple<int,int> GetMouseIndex()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            plane.Raycast(ray, out float distance);
            Vector3 point = ray.GetPoint(distance);
            return new Tuple<int, int>((int)(point.x / CellSizeAsCoordinates + GridSideSize / 2), 
                (int)(point.y / CellSizeAsCoordinates + GridSideSize / 2));
        }

        public enum GameStage
        { First, Second, Thrid }
    }
}