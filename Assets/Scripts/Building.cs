using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Building:MonoBehaviour
    {
        private GameObject model;
        private City cityParent;

        public Building(string path, City city)
        {
            cityParent = city;
            model = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
        }
        public void SetCoordinates(int x, int y, int z)
        {
            model.transform.position = new Vector3(x, y, z);
        }
    }
}