using System;
using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
using Systems.Animation;

namespace Systems.Obstacle
{
    [GameSystem]
    public class FishBrainSystem : GameSystem<BlowFishBrainComponent>
    {
        public override void Register(BlowFishBrainComponent component)
        {
            var collider = component.GetComponent<Collider2D>();
            var animation = component.GetComponent<BlowFishAnimationComponent>();

            collider.OnTriggerStay2DAsObservable()
            .Do(_ => animation.Grow())
            .Throttle(TimeSpan.FromSeconds(5))
            .Subscribe(_ =>
            {
                animation.Shrink();
            })
            .AddTo(component);


        }
    }

}