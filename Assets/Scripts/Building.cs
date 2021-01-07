using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BuildingHelper;

namespace Game
{
    public class Building : MonoBehaviour
    {
        public GameObject Model { get; internal set; }
        internal City cityParent;
        public Projection GridProjection { get; internal set; }

        public Building()
        {}

        public virtual void Build(string path, City city, Tuple<int,int> indexes)
        {
            cityParent = city;
            Model = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
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
        public Home() : base()
        {
            GridProjection = new Projection(new CellState[,]
            { {CellState.Empty,CellState.Fill },
            {CellState.Fill,CellState.Center }});
        }

        public override void Build(string path, City city, Tuple<int, int> indexes)
        {
            base.Build("Home/Home", city, indexes);
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

        public void UpdateSatisfaction()
        {
            for (int i = Mathf.Max(0, GridX - Radius); i < Mathf.Min(GridX + Radius + 1, cityParent.Grid.GetLength(0)); i++)
                for (int j = Mathf.Max(0, GridY - Radius); j < Mathf.Min(GridY + Radius + 1, cityParent.Grid.GetLength(1)); j++)
                    if (cityParent.Grid[i, j] != null && cityParent.Grid[i, j] is Home home) home.Satisfaction[satisfactionName] = true;
        }
    }
    public class Shop : SatisfactionBuilding
    {
        public Shop()
        {
            satisfactionName = "Shop";
            Radius = 5;
            GridProjection = new Projection(new CellState[,]
                {{CellState.Fill,CellState.Center,CellState.Fill},
                {CellState.Fill,CellState.Empty,CellState.Fill }});
        }

        public override void Build(string path,City city, Tuple<int, int> indexes)
        {
            base.Build("Shop", city, indexes);
        }
    }
    public class ScienceCenter : SatisfactionBuilding
    {
        public ScienceCenter()
        {
            satisfactionName = "ScienceCenter";
            Radius = 16;
            GridProjection = new Projection(new CellState[,]
                {{CellState.Empty,CellState.Fill,CellState.Fill},
                {CellState.Fill,CellState.Center, CellState.Fill},
                {CellState.Fill,CellState.Fill,CellState.Empty }});
        }

        public override void Build(string path, City city, Tuple<int, int> indexes)
        {
            base.Build("ScienceCenter", city, indexes);
        }
    }
    public class Park : SatisfactionBuilding
    {
        public Park()
        {
            satisfactionName = "Park";
            Radius = 13;
            GridProjection = new Projection(new CellState[,]
                {{CellState.Fill,CellState.Fill,CellState.Fill,CellState.Fill},
                {CellState.Fill,CellState.Fill,CellState.Fill,CellState.Fill } });
        }

        public override void Build(string path, City city, Tuple<int, int> indexes)
        {
            base.Build("Park", city, indexes);
        }
    }
    public class CarPark : SatisfactionBuilding
    {
        public CarPark()
        {
            satisfactionName = "CarPark";
            Radius = 20;
            GridProjection = new Projection(new CellState[,]
                {{CellState.Fill,CellState.Fill,CellState.Fill},
                {CellState.Fill,CellState.Fill,CellState.Fill },
                {CellState.Fill,CellState.Fill,CellState.Fill }});
        }

        public override void Build(string path, City city, Tuple<int, int> indexes)
        {
            base.Build("CarPark/CarPark", city, indexes);
            Model.transform.position = new Vector3(
                (City.GridSideSize / 2 - indexes.Item1) * City.CellSizeAsCoordinates - City.CellSizeAsCoordinates / 2, 0,
                (City.GridSideSize / 2 - indexes.Item2) * City.CellSizeAsCoordinates - City.CellSizeAsCoordinates / 2);
        }
    }
}