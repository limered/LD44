using SystemBase;
using UnityEngine;

namespace Systems.PlayerUpgrades.Mouth
{
    public class UziComponent : GameComponent
    {
        public GameObject BulletPrefab;
        public GameObject BulletSpawnPoint;
        public float FireInterval = 0.2f;
    }
}