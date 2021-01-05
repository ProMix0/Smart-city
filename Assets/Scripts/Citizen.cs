using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Citizen : MonoBehaviour
    {

        public Dictionary<string, bool> Satisfaction { get; set; } = new Dictionary<string, bool>
        {{ "Home", false }, { "Shop", false },{"ScienceCenter",false },{ "Park", false },{"CarPark",false } };

        
    }
}