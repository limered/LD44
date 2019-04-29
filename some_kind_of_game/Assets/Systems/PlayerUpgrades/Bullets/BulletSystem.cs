using System;
using SystemBase;
using Systems.Movement;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.PlayerUpgrades.Bullets
{
    [GameSystem]
    public class BulletSystem : GameSystem<BulletComponent>
    {
        public override void Register(BulletComponent component)
        {
            var rb2D = component.GetComponent<Rigidbody2D>();
            rb2D.velocity = component.StartVelocity;

            component.OnCollisionEnter2DAsObservable()
                .Subscribe(BulletHit)
                .AddTo(component);

            Observable.Timer(TimeSpan.FromSeconds(2))
                .Subscribe(_ => Object.Destroy(component.gameObject))
                .AddTo(component);
        }

        private void BulletHit(Collision2D collider)
        {
            if (collider.gameObject.GetComponent<AffectedByBullet>())
            {
                collider.gameObject.GetComponent<FishyMovementComponent>()
                    .AddForce(new Vector2(10000, 10000));
            }
        }
    }
}