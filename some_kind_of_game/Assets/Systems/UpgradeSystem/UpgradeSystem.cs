using System;
using SystemBase;
using Systems.Control;
using Systems.PlayerUpgrades.TailFin;
using UniRx;
using Utils.Plugins;

namespace Systems.UpgradeSystem
{
    [GameSystem(typeof(PlayerControlSystem))]
    public class UpgradeSystem : GameSystem<UpgradeConfigComponent, PlayerComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>(null);

        public override void Register(UpgradeConfigComponent component)
        {
            _player.WhereNotNull()
                .Select(player => new {player, upgrades = component})
                .Subscribe(t => OnSubscribeToPlayer(t.player, t.upgrades))
                .AddTo(component);
        }

        private void OnSubscribeToPlayer(PlayerComponent playerComponent, UpgradeConfigComponent upgrades)
        {
            upgrades.UpgradeConfigs.ForEach(config =>
                {
                    if (config.IsAdded.Value)
                    {
                        playerComponent.gameObject.AddComponent(ResolveUpgradeComponentType(config.UpgradeType));
                    }
                });
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        private Type ResolveUpgradeComponentType(UpgradeType type)
        {
            switch (type)
            {
                case UpgradeType.Undefined:
                    throw new ArgumentException("Undefined Type for Upgrade");
                case UpgradeType.Rotor:
                    return typeof(RotorComponent);
                case UpgradeType.Delfin:
                    return typeof(DelfinComponent);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
