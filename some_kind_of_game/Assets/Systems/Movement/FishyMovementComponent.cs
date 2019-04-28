using System;
using System.Linq;
using SystemBase;
using Systems.Movement.Modifier;
using UnityEngine;

namespace Systems.Movement
{
    public class FishyMovementComponent : GameComponent
    {
        public Vector2 AccelerationDefault;
        public Vector2 MaxSpeedDefault;
        public float BackwardFrictionFactorDefault;
        public Action<FishyMovementComponent> HandleInput;
        public GameObject ColliderObject;
        public Vector2 Acceleration { get; set; }

        public Vector2 Velocity { get; set; }

        public Vector2 AccelerationFactor => GetComponents<AccelerationModifier>()
            .Aggregate(AccelerationDefault, (current, modifier) => current + modifier.Summand);

        public Vector2 MaxSpeed => GetComponents<MaxSpeedModifier>()
            .Aggregate(MaxSpeedDefault, (current, modifier) => current + modifier.Summand);

        public float BackwardFrictionFactor => GetComponents<FrictionModifier>()
            .Aggregate(BackwardFrictionFactorDefault, (current, modifier) => current + modifier.Summand);

        public Vector2 ForwardVector { get; set; } = Vector2.right;
    }
}