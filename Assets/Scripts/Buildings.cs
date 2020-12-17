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
            internal LaboratoryAndBeacon(string path, City city) : base(path, city)
            {
                
            }
        }

        public class House : Building
        {
            internal House(string path, City city) : base(path, city)
            {
            }
        }

        public class Factory : Building
        {
            internal Factory(string path, City city) : base(path, city)
            {
            }
        }
    }
}