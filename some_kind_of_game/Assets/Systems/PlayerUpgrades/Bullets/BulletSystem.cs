using SystemBase;
using Systems.Control;
using Systems.Movement.Modifier;
using UniRx;
using UnityEngine;
using Utils.Plugins;

namespace Systems.PlayerUpgrades.Bullet
{
    [GameSystem(typeof(PlayerControlSystem))]
    public class BulletSystem : GameSystem<PlayerComponent, BulletSpawnerComponent, BulletComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>(null);

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        public override void Register(BulletSpawnerComponent component)
        {
            component.OnFire
            .Subscribe(_ =>
            {
                var bullet = GameObject.Instantiate(
                    component.BulletPrefab,
                    component.transform.position,
                    component.transform.rotation
                );

                var bulletCollider = bullet.GetComponent<BulletComponent>();
            })
            .AddTo(component);
        }

        public override void Register(BulletComponent component)
        {
            
        }
    }
}