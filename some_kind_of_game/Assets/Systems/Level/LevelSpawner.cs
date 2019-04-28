using System.Collections.Generic;
using SystemBase;
using UnityEngine;

namespace Systems.Level
{
    public class LevelSpawner : GameComponent
    {
        public int PartsPerLevel = 5;
        public List<GameObject> RoomPartPrefabs = new List<GameObject>();
        public GameObject PlayerSpawnPrefab;
        public GameObject FinishPrefab;
    }
}