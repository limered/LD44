using System;
using System.Collections.Generic;
using System.Linq;
using SystemBase;
using UnityEngine;

namespace Systems.Movement
{
    public class FishyMovementComponent : GameComponent
    {
        public Vector2 Acceleration { get; set; }

        public Vector2 Velocity { get; set; }

        public Vector2 AccelerationDefault;
        public List<Func<Vector2, Vector2>> AccelerationFactorModifier = new List<Func<Vector2, Vector2>>();
        public Vector2 AccelerationFactor {
            get
            {
                var acc = AccelerationDefault;
                foreach (var action in AccelerationFactorModifier)
                {
                    acc = action(acc);
                }

                return acc;
            }
        }

        public Vector2 MaxSpeedDefault;
        public List<Func<Vector2, Vector2>> MaxSpeedModifier = new List<Func<Vector2, Vector2>>();
        public Vector2 MaxSpeed
        {
            get
            {
                var max = MaxSpeedDefault;
                foreach (var action in MaxSpeedModifier)
                {
                    max = action(max);
                }

                return max;
            }
        }

        public float BackwardFrictionFactor;

        public Vector2 ForwardVector { get; set; } = Vector2.right;

        public Action<FishyMovementComponent> HandleInput;

        public GameObject ColliderObject;

    }
}