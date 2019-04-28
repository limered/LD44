using System.Collections.Generic;
using SystemBase;
using Systems.Control;
using Systems.Movement;
using Systems.Movement.Modifier;
using UniRx;
using UnityEngine;
using Utils.Plugins;

namespace Systems.PlayerUpgrades.TailFin
{
    [GameSystem(typeof(PlayerControlSystem))]
    public class TailFinSystem : GameSystem<PlayerComponent, RotorComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>(null);
        private RotorComponent _rotor;

        public override void Register(RotorComponent component)
        {
            _rotor = component;

            _player.WhereNotNull()
                .Subscribe(playerComponent =>
                    {
                        var accModifier = playerComponent.gameObject.AddComponent<AccelerationModifier>();
                        accModifier.Summand = new Vector2(_rotor.AccelerationSummand, 0);

                        var maxSpeedModifier = playerComponent.gameObject.AddComponent<MaxSpeedModifier>();
                        maxSpeedModifier.Summand = new Vector2(_rotor.MaxSpeedSummand, 0);
                    }
                )
                .AddTo(component);
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }
    }
}