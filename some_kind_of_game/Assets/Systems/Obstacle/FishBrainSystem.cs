using System;
using SystemBase;
using Systems.Animation;
using Systems.Control;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Obstacle
{
    [GameSystem]
    public class FishBrainSystem : GameSystem<BlowFishBrainComponent>
    {
        public override void Register(BlowFishBrainComponent component)
        {
            var collider = component.BlowUpCollider;
            var animation = component.GetComponent<BlowFishAnimationComponent>();

            collider.OnTriggerStay2DAsObservable()
                .Where(d => d.attachedRigidbody.GetComponent<PlayerComponent>())
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