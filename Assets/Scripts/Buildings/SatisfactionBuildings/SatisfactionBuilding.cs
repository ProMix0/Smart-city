using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
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
}
