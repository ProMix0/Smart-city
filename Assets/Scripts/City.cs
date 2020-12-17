using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.InGame
{
    public class City
    {
        public List<Building> Buildings { get; } = new List<Building>();
        private Building[,] Grid;
    }
}