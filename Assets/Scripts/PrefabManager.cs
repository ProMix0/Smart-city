using System;
using UnityEngine;

namespace Game
{
    class PrefabManager:ScriptableObject
    {
        public static PrefabManager Manager { get; private set; } = CreateInstance<PrefabManager>();

        public GameObject robotPrefab;
        public GameObject carParkPrefab;
        public GameObject homePrefab;
        public GameObject laboratoryPrefab;
        public GameObject parkPrefab;
        public GameObject roadPrefab;
        public GameObject scienceCenterPrefab;
        public GameObject shopPrefab;
        public GameObject sidewalkPrefab;
    }
}
