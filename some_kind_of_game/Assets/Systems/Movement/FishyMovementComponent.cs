using System;
using System.Collections.Generic;
using System.Linq;
using SystemBase;
using Systems.Movement.Modifier;
using UniRx;
using UnityEngine;

namespace Systems.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FishyMovementComponent : GameComponent
    {
        public Vector2 AccelerationDefault;
        public Vector2 MaxSpeedDefault;
        public float BackwardFrictionFactorDefault;
        public Action<FishyMovementComponent> HandleInput;
        public List<Vector2> Forces = new List<Vector2>();
        public GameObject ColliderObject;
        public Vector2 Acceleration { get; set; }

        public Vector2 Velocity { get; set; }

        public Vector2 AccelerationFactor => GetComponents<AccelerationModifier>()
            .Aggregate(AccelerationDefault, (current, modifier) => current + modifier.Summand);

        public Vector2 MaxSpeed => GetComponents<MaxSpeedModifier>()
            .Aggregate(MaxSpeedDefault, (current, modifier) => current + modifier.Summand);

        public void AddForce(Vector2 force)
        {
            
        }

        public float BackwardFrictionFactor => GetComponents<FrictionModifier>()
            .Aggregate(BackwardFrictionFactorDefault, (current, modifier) => current + modifier.Summand);

        public Vector2 ForwardVector { get; set; } = Vector2.right;

        // Collisions
        public ContactFilter2D ContactFilter { get; set; }
        public Vector2 GroundNormal { get; set; }

        [NonSerialized]
        public ReactiveCommand<RaycastHit2D[]> CollisionDetected = new ReactiveCommand<RaycastHit2D[]>();

        public void AddForce(Vector2 force)
        {
            Forces.Add(force);
        }
    }
}