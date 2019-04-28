using SystemBase;
using Systems.Control;
using Systems.Movement.Modifier;
using UniRx;
using UnityEngine;
using Utils.Plugins;

namespace Systems.PlayerUpgrades.Mouth
{
    [GameSystem(typeof(PlayerControlSystem))]
    public class MouthSystem : GameSystem<PlayerComponent, UziComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>(null);

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        public override void Register(UziComponent component)
        {
            
        }
    }
}