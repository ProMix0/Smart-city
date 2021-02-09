using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game
{
    public class City : MonoBehaviour
    {
        public const int GridSideSize = 50, CellSizeAsCoordinates = 10;
        private GameObject city;

        public Building[,] Grid { get; private set; } = new Building[GridSideSize, GridSideSize];
        public int Radius { get;private set; }
        public List<Building> Buildings { get; private set; } = new List<Building>();
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
            city = GameObject.Find("City");
            float yCoordinate = -0.5f, xCoordinate = GridSideSize * CellSizeAsCoordinates / 2, zCoordinate = xCoordinate;
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
            visualGrid.name = "Grid";
            LineRenderer lineRenderer= visualGrid.AddComponent<LineRenderer>();
            lineRenderer.positionCount = lines.Count;
            lineRenderer.SetPositions(lines.ToArray());


            //city.AddComponent<Park>().Build("", this, new IntStruct(0, 0), null);

            //var test = city.AddComponent<Home>();
            //if (CanBuild(new IntStruct(1, 0), test)) test.Build("", this, new IntStruct(1, 0), null);




            ProceduralGenerating(DateTime.Now.ToString());
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

        public bool CanBuild(IntStruct indexes, Building building)
        {
            int x = indexes.Item1;
            int y = indexes.Item2;
            if (x - building. center.Item1 < 0 ||
                y - building.center.Item2 < 0 ||
                x - building.center.Item1 + building.GridProjection.matrix.GetLength(0)-1 >= Grid.GetLength(0) ||
                y - building.center.Item2 + building.GridProjection.matrix.GetLength(1)-1 >= Grid.GetLength(1)) return false;
            for (int i = x - building.center.Item1; i < x - building.center.Item1 + building.GridProjection.matrix.GetLength(0); i++)
                for (int j = y - building.center.Item2; j < y - building.center.Item2 + building.GridProjection.matrix.GetLength(1); j++)
                    if (building.GridProjection.matrix[i - x + building.center.Item1, j - y + building.center.Item2] != BuildingHelper.CellState.Empty && Grid[i, j] != null)
                        return false;
            return true;
        }

        public void ProceduralGenerating(string seed)
        {
            DateTime milli = DateTime.Now;


            foreach (var item in Buildings)
            {
                Destroy(item.Model);
                Destroy(item);
            }
            Grid = new Building[GridSideSize, GridSideSize];
            Buildings = new List<Building>();

            Debug.Log($"Destroy: {(DateTime.Now - milli).TotalMilliseconds}");
            milli = DateTime.Now;


            System.Random random = new System.Random(seed.GetHashCode());
            int spaceCounter = 0;
            List<int> xSpaces = new List<int>();
            for(int i=0;i< GridSideSize - 3;i++,spaceCounter++)
            {
                if(spaceCounter +random.Next(8)>10+random.Next(3))
                {
                    for(int j=0; j<=GridSideSize;j++)
                    {
                        Road road= city.AddComponent<Road>();
                        Buildings.Add(road);
                        IntStruct tuple = new IntStruct(i, j);
                        if (CanBuild(tuple, road))
                            road.Build("", this, tuple,null);
                    }
                    xSpaces.Add(spaceCounter);
                    spaceCounter = -1;
                }
            }
            xSpaces.Add(spaceCounter+2);
            spaceCounter = 0;
            List<int> zSpaces = new List<int>();
            for (int i = 0; i < GridSideSize - 3; i++, spaceCounter++)
            {
                if (spaceCounter + random.Next(7) > 10 + random.Next(3))
                {
                    for (int j = 0; j <= GridSideSize; j++)
                    {
                        Road road = city.AddComponent<Road>();
                        Buildings.Add(road);
                        IntStruct tuple = new IntStruct(j, i);
                        if (CanBuild(tuple,road))
                            road.Build("", this, tuple,null);
                    }
                    zSpaces.Add(spaceCounter);
                    spaceCounter = -1;
                }
            }
            zSpaces.Add(spaceCounter+2);

            Debug.Log($"Generating roads: {(DateTime.Now - milli).TotalMilliseconds}");

            List<IntStruct[,]> districts = new List<IntStruct[,]>();
            int xSum=0, zSum=0;
            for (int x = 0; x < xSpaces.Count; x++)
            {
                for (int z = 0; z < zSpaces.Count; z++)
                {
                    IntStruct[,] district = new IntStruct[xSpaces[x], zSpaces[z]];
                    for (int i = 0; i < xSpaces[x]; i++)
                        for (int j = 0; j < zSpaces[z]; j++)
                            district[i, j] = new IntStruct(i + xSum + x, j + zSum + z);
                    zSum += zSpaces[z];
                    districts.Add(district);
                }
                zSum = 0;
                xSum += xSpaces[x];
            }

            foreach(var district in districts.OrderBy(a=>random.Next()))
            {
                Building component = GetRandomComponent(random);
                Buildings.Add(component);
                foreach(var indexes in district.AsIEnumerable().OrderBy(a => random.Next()))
                    if(CanBuild(indexes,component))
                    {
                        component.Build("", this, indexes, null);
                        break;
                    }
            }


            int counter = 0;
            Home home = city.AddComponent<Home>();
            Buildings.Add(home);
            do
            {
                var index = new IntStruct(random.Next(GridSideSize), random.Next(GridSideSize));
                if(CanBuild(index,home))
                {
                    home.Build("", this, index, null);
                    home = city.AddComponent<Home>();
                    Buildings.Add(home);
                }
                counter++;
            }
            while (counter + random.Next(GridSideSize / 2) < GridSideSize*2);
        }

        private Building GetRandomComponent(System.Random random)
        {
            Building result=null;
            switch(random.Next(4))
            {
                case 0:
                    result = city.AddComponent<Shop>();
                    break;
                case 1:
                    result = city.AddComponent<ScienceCenter>();
                    break;
                case 2:
                    result = city.AddComponent<Park>();
                    break;
                case 3:
                    result = city.AddComponent<CarPark>();
                    break;
            }
            return result;
        }

        public void OnGenerateClick()
        {
            ProceduralGenerating(GameObject.Find("InputSeed").GetComponent<InputField>().text);
        }

        public struct IntStruct
        {
            public int Item1, Item2;

            public IntStruct(int first, int second)
            {
                Item1 = first;
                Item2 = second;
            }
        }
        public enum GameStage
        { First, Second, Thrid }
    }

    public static class Extensionmethods
    {
        public static IEnumerable<T> AsIEnumerable<T>(this T[,] matrix)
        {
            foreach (var item in matrix)
                yield return item;
        }
    }
}