using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Building : MonoBehaviour
    {
        internal City cityParent;
        public Projection GridProjection { get; internal set; }
        public IntStruct Center { get; protected set; }

        public virtual void Initialize(City city, IntStruct indexes)
        {
            cityParent = city;
            Center = indexes;
            cityParent.Buildings.Add(this);
            try
            {
                for (int i = indexes.Item1 - Center.Item1; i < indexes.Item1 - Center.Item1 + GridProjection.matrix.GetLength(0); i++)
                    for (int j = indexes.Item2 - Center.Item2; j < indexes.Item2 - Center.Item2 + GridProjection.matrix.GetLength(1); j++)
                        if (GridProjection.matrix[i - indexes.Item1 + Center.Item1, j - indexes.Item2 + Center.Item2] != CellState.Empty)
                            cityParent.Grid[i, j] = this;
            }
            catch (Exception)
            {
                Debug.Log("IndexOutOfRangeException in Build()");
            }
        }
    }

    public enum CellState
    { Empty, Fill, Center }

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

        public Projection(bool[,] matrix, int centerX, int centerY)
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