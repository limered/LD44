using System;
using SystemBase;
using Systems.Animation;
using Systems.Control;
using Systems.Movement;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Plugins;

namespace Systems.Obstacle
{
    [GameSystem]
    public class BounceSystem : GameSystem<PlayerComponent, BounceComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>(null);

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        public override void Register(BounceComponent component)
        {

            component.BounceCollider
                .OnTriggerEnter2DAsObservable()
                .Subscribe(coll =>
                {
                    var mov = coll.attachedRigidbody.GetComponent<FishyMovementComponent>();
                    if (mov)
                    {
                        mov.AddForce((mov.transform.position - component.transform.position).normalized * component.Multiplier);
                    }
                })
                .AddTo(component);
        }
    }

}