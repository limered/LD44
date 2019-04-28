using SystemBase;
using Systems.Movement;
using UnityEngine;

namespace Systems.ActiveOstacles
{
    [RequireComponent(typeof(FishyMovementComponent))]
    public class WhaleComponent : GameComponent
    {
        public Vector2 SwimDirection = Vector2.left;
        public float Acceleration = 5;
    }
}
