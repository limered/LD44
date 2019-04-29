using SystemBase;
using Systems.Level;
using UnityEngine;

namespace Systems.Obstacle
{
    public class FishSpawnerComponent : GameComponent
    {
        public GameObject[] SpawnPrefabs;

        public uint Amount = 1;

        [Range(0, 100)]
        public float Radius = 10f;

        public RoomComponent Room;
        public Collider2D TriggerOverwrite;
    }
}
