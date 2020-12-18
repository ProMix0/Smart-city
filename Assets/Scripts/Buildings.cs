using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Assets.InGame
{
    public static class Buildings
    {
        public class LaboratoryAndBeacon :Building
        {
            public LaboratoryAndBeacon(City city, string path) : base(city, path,5,5)
            {
                Dictionary<Resource, int> maxResources = new Dictionary<Resource, int>();
                foreach(var str in Enum.GetNames(typeof(Resource)))
                {
                    maxResources[(Resource)Enum.Parse(typeof(Resource), str)] = 1000;
                }
                Parts.Add(new ResourceStorage(this, maxResources));
            }
        }

        public class House : Building
        {
            public House(City city, string path) : base(city, path,1,2)
            {
                Parts.Add(new InGame.House(this, 10));
            }
        }

        public class Factory : Building
        {
            public Factory(City city, FactoryType type, Dictionary<FactoryType,string> paths) : base(city,paths[type],3,3)
            {
                //TODO change pollution by factory type
                //TODO change consuming by factory type
                //TODO change producting by factory type
                //TODO change storable resources by factory type
                //TODO change citizen using by factory type
            }
            public enum FactoryType
            {

            }
        }

        public class Park : Building
        {
            public Park(City city, string path) : base(city, path,6,2)
            {
                Parts.Add(new RestPoint(this, 40, 5));
            }
        }

        public class Road : Building
        {
            public Road(City city, string path) : base(city, path, 1, 1)
            {
                //TODO implement TransportNet
            }
        }

        public class Shop : Building
        {
            public Shop(City city, string path) : base(city, path, 2, 2)
            {
                Dictionary<Resource, int> maxResources = new Dictionary<Resource, int>();
                foreach (var str in Enum.GetNames(typeof(Resource)))
                {
                    maxResources[(Resource)Enum.Parse(typeof(Resource), str)] = 1000;
                }
                Parts.Add(new ResourceStorage(this, maxResources));
                //TODO what resources must be consumed?
            }
        }
    }
}