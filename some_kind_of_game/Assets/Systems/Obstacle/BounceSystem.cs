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
            _player.WhereNotNull()
            .Subscribe(player =>
            {
                component.BounceCollider
                    .OnTriggerEnter2DAsObservable()
                    .Subscribe(coll =>
                    {
                        var mov = player.GetComponent<FishyMovementComponent>();
                        mov.Acceleration = new Vector2(-1000, 1000);
                    })
                    .AddTo(component);
            })
            .AddTo(component);
        }
    }

}