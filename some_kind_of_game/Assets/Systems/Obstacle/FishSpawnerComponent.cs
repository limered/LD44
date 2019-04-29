using SystemBase;
using Systems.Level;
using UnityEngine;

namespace Systems.Obstacle
{
    public class FishSpawnerComponent : GameComponent
    {
        public GameObject SopawnPrefab;
        public RoomComponent Room;
        public Collider2D TriggerOverwrite;
    }
}
