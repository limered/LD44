using System;
using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx;
using UnityEngine;
using Systems.Animation;
using SystemBase.StateMachineBase;
using UniRx.Triggers;

namespace Systems.Obstacle
{
    public class BounceComponent : GameComponent
    {
        public float Multiplier = 10000;
        [Header("can be set manually, otherwise fetched from children")]
        public Collider2D BounceCollider;

    }

}