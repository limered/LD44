using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UnityEngine;

namespace Systems.Animation
{
    public class AlgaeAnimationComponent : GameComponent
    {
        public Vector3 RotationAxis = Vector3.forward;

        [Range(-360, 360)]
        public int MinAngle = -20;

        [Range(-360, 360)]
        public int MaxAngle = 20;
        public float WobbleSpeed = 6;

        public float CurrentAngle { get; set; }
        public AlgaeDirection Direction { get; set; } = AlgaeDirection.Left;
    }

    public enum AlgaeDirection
    {
        Left = -1,
        Right = 1
    }
}