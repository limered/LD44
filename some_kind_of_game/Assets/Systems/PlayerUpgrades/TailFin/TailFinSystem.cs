using SystemBase;
using Systems.Control;
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

        public override void Register(RotorComponent component)
        {
            _player.WhereNotNull()
                .Select(player => new { player, rotor = component })
                .Subscribe(t => AddRotorModifiersToPlayer(t.player, t.rotor))
                .AddTo(component);
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        private static void AddRotorModifiersToPlayer(Component player, RotorComponent rotor)
        {
            var accModifier = player.gameObject.AddComponent<AccelerationModifier>();
            accModifier.Summand = new Vector2(rotor.AccelerationSummand, 0);

            var maxSpeedModifier = player.gameObject.AddComponent<MaxSpeedModifier>();
            maxSpeedModifier.Summand = new Vector2(rotor.MaxSpeedSummand, 0);
        }
    }
}