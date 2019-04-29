using System;
using SystemBase;
using Systems.Control;
using StrongSystems.Audio;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

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
            Observable.Interval(TimeSpan.FromMilliseconds(component.FireInterval))
                .Subscribe(l => Shoot(component))
                .AddTo(component);
        }

        private static void Shoot(UziComponent component)
        {
            Object.Instantiate(component.BulletPrefab, component.BulletSpawnPoint.transform.position, Quaternion.identity);
            "Uzi".Play();
        }
    }
}