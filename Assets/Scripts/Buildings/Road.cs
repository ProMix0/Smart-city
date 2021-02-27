using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Road : Building
    {
        public static readonly Projection defaultProjection= new Projection(new CellState[,]
                {{CellState.Center }});

        public void Awake()
        {
            GridProjection = defaultProjection;
            Center = new IntStruct(0, 0);
        }

        public override void Initialize(City city, IntStruct indexes)
        {
            base.Initialize(city, indexes);
        }
    }
}
