using SystemBase;
using Systems.Movement;
using UnityEngine;

namespace Systems.Obstacle
{
    [RequireComponent(typeof(Rigidbody2D), typeof(FishyMovementComponent))]
    public class SimpleFishBrainComponent : GameComponent
    {
        public float Multiplier = 1;
        public Vector2 Direction = Vector2.left;
    }
}
