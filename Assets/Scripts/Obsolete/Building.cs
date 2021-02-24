using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BuildingHelper;
using static Game.City;

namespace Game
{
    public class Building : MonoBehaviour
    {
        public GameObject model;
        internal City cityParent;
        public Projection GridProjection { get; internal set; }
        public Tuple<int, int> center;

        public virtual void Build(string path, City city, IntStruct indexes, Building building)
        {
            cityParent = city;
            model = (GameObject)Instantiate(Resources.Load($"{path}/{path}"));
            try
            {
                for (int i = indexes.Item1 - center.Item1; i < indexes.Item1 - center.Item1 + GridProjection.matrix.GetLength(0); i++)
                    for (int j = indexes.Item2 - center.Item2; j < indexes.Item2 - center.Item2 + GridProjection.matrix.GetLength(1); j++)
                        if (GridProjection.matrix[i - indexes.Item1 + center.Item1, j - indexes.Item2 + center.Item2] != CellState.Empty)
                            cityParent.Grid[i, j] = building;
            }
            catch (Exception ex)
            {
                Debug.Log("IndexOutOfRangeException in Build()");
            }
        }

        

        public class Projection
        {
            public readonly CellState[,] matrix;
            public readonly int centerX, centerY;

            public Projection(CellState[,] matrix)
            {
                if (centerX < 0 || centerX >= matrix.GetLength(0)) throw new ArgumentOutOfRangeException();
                if (centerY < 0 || centerY >= matrix.GetLength(1)) throw new ArgumentOutOfRangeException();
                this.matrix = matrix;
                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                        if (matrix[i, j] == CellState.Center)
                        {
                            centerX = i;
                            centerY = j;
                        }
            }

            public Projection(bool[,] matrix, int centerX,int centerY)
            {
                if (centerX < 0 || centerX >= matrix.GetLength(0)) throw new ArgumentOutOfRangeException();
                if (centerY < 0 || centerY >= matrix.GetLength(1)) throw new ArgumentOutOfRangeException();
                this.matrix = new CellState[matrix.GetLength(0), matrix.GetLength(1)];
                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                        this.matrix[i, j] = matrix[i, j] ? CellState.Fill : CellState.Empty;
                this.matrix[centerX, centerY] = CellState.Center;
            }
        }
    }

    public class Home : Building
    {
        public List<Citizen> Citizens { get; } = new List<Citizen>();
        public Dictionary<string, bool> Satisfaction { get; } = new Dictionary<string, bool>
        { { "Shop", false },{"ScienceCenter",false },{ "Park", false },{"CarPark",false } };
        public void Awake()
        {
            GridProjection = new Projection(new CellState[,]
            { {CellState.Fill,CellState.Empty },
            {CellState.Center,CellState.Fill }});
            center = new Tuple<int, int>(1, 0);
        }

        public override void Build(string path, City city, IntStruct indexes, Building building)
        {
            base.Build("Home", city, indexes, this);
            for (int i = 0; i < 3 && city.FreeCitizens.Count > 0; i++)
                Citizens.Add(city.FreeCitizens.Dequeue());
            model.transform.position = new Vector3(
                (GridSideSize / 2 - indexes.Item1) * CellSizeAsCoordinates + CellSizeAsCoordinates / 2 - 7, 0,
                (GridSideSize / 2 - indexes.Item2) * CellSizeAsCoordinates - CellSizeAsCoordinates * 1.5f + 4);
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

    public class Road : Building
    {
        public void Awake()
        {
            GridProjection = new Projection(new CellState[,]
                {{CellState.Center }});
            center = new Tuple<int, int>(0, 0);
        }

        public override void Build(string path, City city, IntStruct indexes, Building building)
        {
            base.Build("Road", city, indexes, this);
            model.transform.position = new Vector3(
                (GridSideSize / 2 - indexes.Item1) * CellSizeAsCoordinates - CellSizeAsCoordinates / 2, 0,
                (GridSideSize / 2 - indexes.Item2) * CellSizeAsCoordinates - CellSizeAsCoordinates / 2);
        }
    }

    public class Sidewalk : Building
    {
        public void Awake()
        {
            GridProjection = new Projection(new CellState[,]
                {{CellState.Center }});
            center = new Tuple<int, int>(0, 0);
        }

        public override void Build(string path, City city, IntStruct indexes, Building building)
        {
            base.Build("Sidewalk", city, indexes, this);
            model.transform.position = new Vector3(
                (GridSideSize / 2 - indexes.Item1) * CellSizeAsCoordinates - CellSizeAsCoordinates / 2, 0,
                (GridSideSize / 2 - indexes.Item2) * CellSizeAsCoordinates - CellSizeAsCoordinates / 2);
        }
    }

    public class SatisfactionBuilding : Building
    {
        public int Radius { get; internal set; }
        public int GridX { get; set; }
        public int GridY { get; set; }
        internal string satisfactionName;

        public void UpdateSatisfaction()
        {
            for (int i = Mathf.Max(0, GridX - Radius); i < Mathf.Min(GridX + Radius + 1, cityParent.Grid.GetLength(0)); i++)
                for (int j = Mathf.Max(0, GridY - Radius); j < Mathf.Min(GridY + Radius + 1, cityParent.Grid.GetLength(1)); j++)
                    if (cityParent.Grid[i, j] != null && cityParent.Grid[i, j] is Home home) home.Satisfaction[satisfactionName] = true;
        }
    }
    public class Shop : SatisfactionBuilding
    {
        public void Awake()
        {
            satisfactionName = "Shop";
            Radius = 5;
            GridProjection = new Projection(new CellState[,]
                {{CellState.Fill,CellState.Fill},
                {CellState.Empty,CellState.Center},
                {CellState.Fill,CellState.Fill}});
            center = new Tuple<int, int>(1, 1);
        }

        public override void Build(string path,City city, IntStruct indexes, Building building)
        {
            base.Build("Shop", city, indexes, building);
            model.transform.position = new Vector3(
                (GridSideSize / 2 - indexes.Item1) * CellSizeAsCoordinates - CellSizeAsCoordinates / 2+6, 0,
                (GridSideSize / 2 - indexes.Item2) * CellSizeAsCoordinates - CellSizeAsCoordinates / 2+9);
        }
    }
    public class ScienceCenter : SatisfactionBuilding
    {
        public void Awake()
        {
            satisfactionName = "ScienceCenter";
            Radius = 16;
            GridProjection = new Projection(new CellState[,]
                {{CellState.Fill,CellState.Fill,CellState.Empty},
                {CellState.Fill,CellState.Center, CellState.Fill},
                {CellState.Empty,CellState.Fill,CellState.Fill }});
            center = new Tuple<int, int>(1, 1);
        }

        public override void Build(string path, City city, IntStruct indexes, Building building)
        {
            base.Build("ScienceCenter", city, indexes, this);
            model.transform.position = new Vector3(
                (GridSideSize / 2 - indexes.Item1) * CellSizeAsCoordinates - CellSizeAsCoordinates / 2+1, 0,
                (GridSideSize / 2 - indexes.Item2) * CellSizeAsCoordinates - CellSizeAsCoordinates / 2+7.5f);
        }
    }
    public class Park : SatisfactionBuilding
    {
        public void Awake()
        {
            satisfactionName = "Park";
            Radius = 13;
            GridProjection = new Projection(new CellState[,]
                {{CellState.Center,CellState.Fill},
                {CellState.Fill,CellState.Fill},
                {CellState.Fill,CellState.Fill} ,
                {CellState.Fill,CellState.Fill}});
            center = new Tuple<int, int>(0, 0);
        }

        public override void Build(string path, City city, IntStruct indexes, Building building)
        {
            base.Build("Park", city, indexes, this);
            model.transform.position = new Vector3(
                (GridSideSize / 2 - indexes.Item1) * CellSizeAsCoordinates -2*CellSizeAsCoordinates, 0,
                (GridSideSize / 2 - indexes.Item2) * CellSizeAsCoordinates - CellSizeAsCoordinates);
        }
    }
    public class CarPark : SatisfactionBuilding
    {
        public void Awake()
        {
            satisfactionName = "CarPark";
            Radius = 20;
            GridProjection = new Projection(new CellState[,]
                {{CellState.Fill,CellState.Fill,CellState.Fill},
                {CellState.Fill,CellState.Center,CellState.Fill },
                {CellState.Fill,CellState.Fill,CellState.Fill }});
            center = new Tuple<int, int>(1, 1);
        }

        public override void Build(string path, City city, IntStruct indexes, Building building)
        {
            base.Build("CarPark", city, indexes, this);
            model.transform.position = new Vector3(
                (GridSideSize / 2 - indexes.Item1) * CellSizeAsCoordinates - CellSizeAsCoordinates / 2, 0,
                (GridSideSize / 2 - indexes.Item2) * CellSizeAsCoordinates - CellSizeAsCoordinates / 2);
        }
    }
}