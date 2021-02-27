using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Shop : SatisfactionBuilding
    {

        public static readonly Projection defaultProjection = new Projection(new CellState[,]
                {{CellState.Fill,CellState.Fill},
                {CellState.Empty,CellState.Center},
                {CellState.Fill,CellState.Fill}});

        public void Awake()
        {
            satisfactionName = "Shop";
            Radius = 5;
            GridProjection = new Projection(new CellState[,]
                {{CellState.Fill,CellState.Fill},
                {CellState.Empty,CellState.Center},
                {CellState.Fill,CellState.Fill}});
            Center = new IntStruct(1, 1);
        }

        public override void Initialize(City city, IntStruct indexes)
        {
            base.Initialize(city, indexes);
        }
    }
}
