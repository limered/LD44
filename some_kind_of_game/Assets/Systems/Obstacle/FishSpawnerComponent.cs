using SystemBase;
using Systems.Level;
using UnityEngine;

namespace Systems.Obstacle
{
    public class FishSpawnerComponent : GameComponent
    {
        public GameObject[] SpawnPrefabs;
        public RoomComponent Room;
        public Collider2D TriggerOverwrite;
    }
}
