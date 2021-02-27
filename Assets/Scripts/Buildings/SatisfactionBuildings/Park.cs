using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Park : SatisfactionBuilding
    {

        public static readonly Projection defaultProjection = new Projection(new CellState[,]
                {{CellState.Center,CellState.Fill},
                {CellState.Fill,CellState.Fill},
                {CellState.Fill,CellState.Fill} ,
                {CellState.Fill,CellState.Fill}});

        public void Awake()
        {
            satisfactionName = "Park";
            Radius = 13;
            GridProjection = new Projection(new CellState[,]
                {{CellState.Center,CellState.Fill},
                {CellState.Fill,CellState.Fill},
                {CellState.Fill,CellState.Fill} ,
                {CellState.Fill,CellState.Fill}});
            Center = new IntStruct(0, 0);
        }

        public override void Initialize(City city, IntStruct indexes)
        {
            base.Initialize(city, indexes);
        }
    }
}
