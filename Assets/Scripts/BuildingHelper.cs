using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class BuildingHelper : MonoBehaviour
    {
        private CellState[,] matrix;
        private City city;

        public BuildingHelper(bool[,] matrix, int centerX, int centerY, City city)
        {
            if (centerX < 0 || centerX >= matrix.GetLength(0)) throw new ArgumentOutOfRangeException();
            if (centerY < 0 || centerY >= matrix.GetLength(1)) throw new ArgumentOutOfRangeException();
            this.city = city;
            this.matrix = new CellState[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    this.matrix[i, j] = matrix[i, j] ? CellState.Fill : CellState.Empty;
            this.matrix[centerX, centerY] = CellState.Center;
        }

        private Tuple<int,int> CenterIndexes()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] == CellState.Center) return new Tuple<int, int>(i, j);
            throw new InvalidOperationException();
        }
        private bool CanBuild(int x, int y)
        {
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
            //Get cursor grid position
            //Build by position
        }

        public void FixedUpdate()
        {
            //Get cursor grid coordinates
            //Move build zone
            //Check ability to build there
            //Change color of build zone
        }

        private enum CellState
        { Empty,Fill,Center}
    }
}