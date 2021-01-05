using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class BuildingHelper : MonoBehaviour
    {
        private CellState[,] matrix;
        private City city;
        private Building building;

        public BuildingHelper(Building toBuild, City city)
        {
            matrix = toBuild.GridProjection.matrix;
            building = toBuild;
        }

        public Tuple<int,int> CenterIndexes()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] == CellState.Center) return new Tuple<int, int>(i, j);
            throw new InvalidOperationException();
        }
        private bool CanBuild()
        {
            var position = City.GetMouseIndex();
            int x = position.Item1;
            int y = position.Item2;
            var center = CenterIndexes();
            if (x - center.Item1 < 0 ||
                y - center.Item2 < 0 ||
                x - center.Item1 + matrix.GetLength(0) >= city.Grid.GetLength(0) ||
                y - center.Item2 + matrix.GetLength(1) >= city.Grid.GetLength(1)) return false;
            for (int i = x - center.Item1; i < x - center.Item1 + matrix.GetLength(0); i++)
                for (int j = y - center.Item2; j < y - center.Item2 + matrix.GetLength(1); j++)
                    if (matrix[i - x + center.Item1, j - y + center.Item2] != CellState.Empty && city.Grid[i, j] != null) return false;
            return true;
        }

        public void Rotate()
        {
            var result = new CellState[matrix.GetLength(1), matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(1); i++)
                for (int j = 0; j < matrix.GetLength(0); j++)
                    result[i, j] = matrix[matrix.GetLength(0) - j - 1, i];

            matrix = result;
        }

        public void Build()
        {
            if (CanBuild())
            {
                //Get cursor grid position
                var index = City.GetMouseIndex();
                //Build by position
                var position = City.GetMouseIndex();
                int x = position.Item1;
                int y = position.Item2;
                var center = CenterIndexes();
                for (int i = x - center.Item1; i < x - center.Item1 + matrix.GetLength(0); i++)
                    for (int j = y - center.Item2; j < y - center.Item2 + matrix.GetLength(1); j++)
                        if (matrix[i - x + center.Item1, j - y + center.Item2] != CellState.Empty) city.Grid[i, j] = building;

                building.Build("",city);
            }
        }

        public void FixedUpdate()
        {
            //Get cursor grid coordinates
            //Move build zone
            //Check ability to build there
            //Change color of build zone
        }

        public enum CellState
        { Empty,Fill,Center}
    }
}