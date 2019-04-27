using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UnityEngine;

namespace Systems.Animation
{
    public class FinAnimationComponent : GameComponent
    {
        [Range(0, 360)]
        public int SpreadAngle = 90;
        public float WobbleSpeed = 10;


        public float CurrentAngle { get; set; }
        public FinDirection Direction { get; set; } = FinDirection.Left;
    }

    public enum FinDirection
    {
        Left = -1,
        Right = 1
    }
}