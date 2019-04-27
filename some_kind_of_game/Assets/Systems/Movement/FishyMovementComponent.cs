using System;
using SystemBase;
using UnityEngine;

namespace Systems.Movement
{
    public class FishyMovementComponent : GameComponent
    {
        public Vector2 Acceleration { get; set; }

        public Vector2 Velocity { get; set; }

        public Vector2 AccelerationFactor;
        public Vector2 MaxSpeed;

        public float BackwardFrictionFactor;

        public Vector2 ForwardVector { get; set; } = Vector2.right;

        public Action<FishyMovementComponent> HandleInput;

        public GameObject ColliderObject;

    }
}