using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Movement
{
    [GameSystem]
    public class FishyMovementSystem : GameSystem<FishyMovementComponent>
    {
        public override void Register(FishyMovementComponent component)
        {
            component.FixedUpdateAsObservable()
                .Select(_ => component)
                .Subscribe(OnFishMove)
                .AddTo(component);
        }

        private static void OnFishMove(FishyMovementComponent component)
        {
            component.HandleInput(component);
            ApplyFriction(component);
            Animate(component);
            ApplyAnimationToModel(component);
            FixCollider(component);
        }

        private static void Animate(FishyMovementComponent component)
        {
            var nextVelocity = component.Velocity + component.Acceleration * UnityEngine.Time.fixedDeltaTime;

            if (Mathf.Abs(component.Velocity.x) < component.MaxSpeed.x)
            {
                component.Velocity = new Vector2(nextVelocity.x, component.Velocity.y);
            }
            if (Mathf.Abs(component.Velocity.y) < component.MaxSpeed.y)
            {
                component.Velocity = new Vector2(component.Velocity.x, nextVelocity.y);
            }

        }

        private static void ApplyFriction(FishyMovementComponent component)
        {
            var backFricktion = component.Velocity * -component.BackwardFrictionFactor;
            component.Velocity = component.Velocity + backFricktion * UnityEngine.Time.fixedDeltaTime;
        }

        private static void ApplyAnimationToModel(FishyMovementComponent component)
        {
            var positionChange = component.Velocity * UnityEngine.Time.fixedDeltaTime;
            
            component.transform.position = new Vector3(
                component.transform.position.x + positionChange.x,
                component.transform.position.y + positionChange.y,
                component.transform.position.z);
        }

        private static void FixCollider(FishyMovementComponent component)
        {
            if(component.ColliderObject)
            {
                component.ColliderObject.transform.localPosition = Vector3.zero;
            }
        }
    }
}
