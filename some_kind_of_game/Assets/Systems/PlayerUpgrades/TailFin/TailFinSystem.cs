using SystemBase;
using Systems.Control;
using Systems.Movement;
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
                .Select(playerComponent => playerComponent.GetComponent<FishyMovementComponent>())
                .Subscribe(movement =>
                    {
                        movement.AccelerationFactorModifier
                            .Add(old => new Vector2(old.x + _rotor.AccelerationSummand, old.y));

                        movement.MaxSpeedModifier
                            .Add(old => new Vector2(old.x + _rotor.MaxSpeedSummand, old.y));
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