using SystemBase;
using UniRx;
using UnityEngine;
using Utils.Plugins;

namespace Systems.PlayerUpgrades.Bullet
{
    public class BulletSpawnerComponent : GameComponent
    {
        public float ShootSpeed = 100;
        public GameObject BulletPrefab;
        public readonly ReactiveProperty<Unit> OnFire = new ReactiveProperty<Unit>();

        public void Fire(){
            OnFire.Fire();
        }
    }
}