using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Home : Building
    {
        public List<Citizen> Citizens { get; } = new List<Citizen>();
        public Dictionary<string, bool> Satisfaction { get; } = new Dictionary<string, bool>
        { { "Shop", false },{"ScienceCenter",false },{ "Park", false },{"CarPark",false } };

        public static readonly Projection defaultProjection=new Projection(new CellState[,]
            { {CellState.Fill,CellState.Empty
    },
            {CellState.Center,CellState.Fill
}});

public void Awake()
        {
            GridProjection = defaultProjection;
            Center = new IntStruct(1, 0);
        }

        public override void Initialize(City city, IntStruct indexes)
        {
            base.Initialize(city, indexes);
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
}
