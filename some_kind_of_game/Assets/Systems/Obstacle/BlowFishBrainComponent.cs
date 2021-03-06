using System;
using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx;
using UnityEngine;
using Systems.Animation;

namespace Systems.Obstacle
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(BlowFishAnimationComponent))]
    public class BlowFishBrainComponent : GameComponent
    {
        public Collider2D BlowUpCollider;
        public Collider2D BounceCollider;
    }

}