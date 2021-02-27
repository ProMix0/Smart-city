using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static Game.ExtensionMethods;

namespace Game
{
    public class City : MonoBehaviour
    {
        public const int GridSideSize = 50, CellSizeAsCoordinates = 10;
        private GameObject city;

        public Building[,] Grid { get; private set; } = new Building[GridSideSize, GridSideSize];
        public int Radius { get; private set; }
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
                switch (Stage)
                {
                    case GameStage.Thrid:
                        max += 3;
                        foreach (var item in Citizens)
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
            GameObject visualGrid = new GameObject
            {
                name = "Grid"
            };
            LineRenderer lineRenderer = visualGrid.AddComponent<LineRenderer>();
            lineRenderer.positionCount = lines.Count;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPositions(lines.ToArray());


            //city.AddComponent<Home>().Build("", this, new IntStruct(0, 0), null);

            //var test = city.AddComponent<Home>();
            //IntStruct coords = new IntStruct(1, 1);
            //if (true || CanBuild(coords, test)) test.Build("", this, coords, null);




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


        public void ProceduralGenerating(string seed)
        {
            DateTime milli = DateTime.Now;


            foreach (var item in Buildings.Select(building => building.gameObject))
            {
                Destroy(item);
            }
            Grid = new Building[GridSideSize, GridSideSize];
            Buildings = new List<Building>();

            Debug.Log($"Destroy: {(DateTime.Now - milli).TotalMilliseconds}");
            milli = DateTime.Now;


            System.Random random = new System.Random(seed.GetHashCode());
            int spaceCounter = 0;
            List<int> xSpaces = new List<int>();
            for (int i = 0; i < GridSideSize - 3; i++, spaceCounter++)
            {
                if (spaceCounter + random.Next(8) > 10 + random.Next(3))
                {
                    for (int j = 0; j <= GridSideSize; j++)
                    {
                        IntStruct indexes = new IntStruct(i, j);
                        if (CanPlace(this, indexes, Road.defaultProjection))
                            Instantiate(PrefabManager.Manager.roadPrefab)
                                .GetComponent<Road>().Initialize(this, indexes);

                        //Road road = city.AddComponent<Road>();
                        //Buildings.Add(road);
                        //IntStruct tuple = new IntStruct(i, j);
                        //if (CanBuild(tuple, road))
                        //    road.Build("", this, tuple, null);
                    }
                    xSpaces.Add(spaceCounter);
                    spaceCounter = -1;
                }
            }
            xSpaces.Add(spaceCounter + 2);
            spaceCounter = 0;
            List<int> zSpaces = new List<int>();
            for (int i = 0; i < GridSideSize - 3; i++, spaceCounter++)
            {
                if (spaceCounter + random.Next(7) > 10 + random.Next(3))
                {
                    for (int j = 0; j <= GridSideSize; j++)
                    {
                        IntStruct indexes = new IntStruct(i, j);
                        if (CanPlace(this, indexes, Road.defaultProjection))
                            Instantiate(PrefabManager.Manager.roadPrefab)
                                .GetComponent<Road>().Initialize(this, indexes);
                    }
                    zSpaces.Add(spaceCounter);
                    spaceCounter = -1;
                }
            }
            zSpaces.Add(spaceCounter + 2);

            for (int i = 0; i < Grid.GetLength(0); i++)
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    if (i == 0 || Grid[i - 1, j] is Road
                        || i + 1 == Grid.GetLength(0) || Grid[i + 1, j] is Road
                        || j == 0 || Grid[i, j - 1] is Road
                        || j + 1 == Grid.GetLength(1) || Grid[i, j + 1] is Road)
                    {
                        IntStruct indexes = new IntStruct(i, j);
                        if (CanPlace(this, indexes, Sidewalk.defaultProjection))
                            Instantiate(PrefabManager.Manager.sidewalkPrefab)
                                .GetComponent<Sidewalk>().Initialize(this, indexes);
                    }
                }
            Graph<Sidewalk> sidewalksGraph = new Graph<Sidewalk>();
            for (int i = 0; i < Grid.GetLength(0); i++)
                for (int j = 0; j < Grid.GetLength(1); j++)
                    if (Grid[i, j] is Sidewalk sidewalk)
                    {
                        var newNode = sidewalksGraph.AddNode(sidewalk);
                        if (i > 0 && Grid[i - 1, j] is Sidewalk sidewalkLeft)
                        {
                            var nodeLeft = sidewalksGraph.Nodes.Where(node => node.Item == sidewalkLeft).FirstOrDefault();
                            if (nodeLeft != null) sidewalksGraph.Bind(newNode, nodeLeft);
                        }
                        if (j > 0 && Grid[i, j - 1] is Sidewalk sidewalkUp)
                        {
                            var nodeUp = sidewalksGraph.Nodes.Where(node => node.Item == sidewalkUp).FirstOrDefault();
                            if (nodeUp != null) sidewalksGraph.Bind(newNode, nodeUp);
                        }
                    }

            foreach (var node in sidewalksGraph.Nodes.OrderBy(node => random.Next()).Take(random.Next(10, 20)))
            {
                //Citizen generating
            }


            Debug.Log($"Generating roads: {(DateTime.Now - milli).TotalMilliseconds}");

            List<IntStruct[,]> districts = new List<IntStruct[,]>();
            int xSum = 0, zSum = 0;
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

            foreach (var district in districts.OrderBy(a => random.Next()))
            {
                foreach (var index in district)
                    if (BuildRandomBuilding(random, index))
                        break;
            }


            int counter = 0;
            do
            {
                var index = new IntStruct(random.Next(GridSideSize), random.Next(GridSideSize));

                if (CanPlace(this, index, Home.defaultProjection))
                    Instantiate(PrefabManager.Manager.homePrefab)
                        .GetComponent<Home>().Initialize(this, index);

                counter++;
            }
            while (counter + random.Next(GridSideSize / 2) < GridSideSize * 2);
        }

        private bool BuildRandomBuilding(System.Random random, IntStruct index)
        {
            bool result = false;
            switch (random.Next(4))
            {
                case 0:
                    if (CanPlace(this, index, CarPark.defaultProjection))
                    {
                        Instantiate(PrefabManager.Manager.carParkPrefab)
                            .GetComponent<CarPark>().Initialize(this, index);
                        result = true;
                    }
                    break;
                case 1:
                    if (CanPlace(this, index, Park.defaultProjection))
                    {
                        Instantiate(PrefabManager.Manager.parkPrefab)
                            .GetComponent<Park>().Initialize(this, index);
                        result = true;
                    }
                    break;
                case 2:
                    if (CanPlace(this, index, ScienceCenter.defaultProjection))
                    {
                        Instantiate(PrefabManager.Manager.scienceCenterPrefab)
                            .GetComponent<ScienceCenter>().Initialize(this, index);
                        result = true;
                    }
                    break;
                case 3:
                    if (CanPlace(this, index, Shop.defaultProjection))
                    {
                        Instantiate(PrefabManager.Manager.shopPrefab)
                            .GetComponent<Shop>().Initialize(this, index);
                        result = true;
                    }
                    break;
            }
            return result;
        }

        public void OnGenerateClick()
        {
            ProceduralGenerating(GameObject.Find("InputSeed").GetComponent<InputField>().text);
        }

        
        public enum GameStage
        { First, Second, Thrid }
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

    public static class ExtensionMethods
    {
        public static IEnumerable<T> AsIEnumerable<T>(this T[,] matrix)
        {
            foreach (var item in matrix)
                yield return item;
        }

        public static Tuple<int, int> GetIndexesByCoordinates(float x, float y)
        {
            return new Tuple<int, int>((int)(x / City.CellSizeAsCoordinates + City.GridSideSize / 2),
                (int)(y / City.CellSizeAsCoordinates + City.GridSideSize / 2));
        }

        public static Tuple<int, int> GetIndexesByCoordinates(Vector3 point)
        {
            return GetIndexesByCoordinates(point.x, point.z);
        }

        public static Tuple<int, int> GetMouseIndex()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            plane.Raycast(ray, out float distance);
            Vector3 point = ray.GetPoint(distance);
            return GetIndexesByCoordinates(point.x, point.y);
        }

        public static bool CanPlace(City city,IntStruct indexes, Projection projection)
        {
            int x = indexes.Item1;
            int y = indexes.Item2;
            if (x - projection.centerX < 0 ||
                y - projection.centerY < 0 ||
                x - projection.centerX + projection.matrix.GetLength(0) - 1 >= city.Grid.GetLength(0) ||
                y - projection.centerY + projection.matrix.GetLength(1) - 1 >= city.Grid.GetLength(1)) return false;
            for (int i = x - projection.centerX; i < x - projection.centerX + projection.matrix.GetLength(0); i++)
                for (int j = y - projection.centerY; j < y - projection.centerY + projection.matrix.GetLength(1); j++)
                    if (projection.matrix[i - x + projection.centerX, j - y + projection.centerY]
                        != CellState.Empty && city.Grid[i, j] != null)
                        return false;
            return true;
        }
    }
}