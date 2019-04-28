using System;
using System.Linq;
using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Unity;

namespace Systems.Movement
{
    [GameSystem(typeof(Systems.Control.PlayerControlSystem))]
    public class FishyMovementSystem : GameSystem<FishyMovementComponent>
    {
        private const float CollisionShell = 0.01f;
        private const float MinGroundNormalY = 0.65f;

        public override void Register(FishyMovementComponent component)
        {
            component.FixedUpdateAsObservable()
                .Select(_ => component)
                .Subscribe(OnFishMove)
                .AddTo(component);

            var contactFilter = new ContactFilter2D
            {
                useTriggers = false,
                useLayerMask = true
            };
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(component.gameObject.layer));
            component.ContactFilter = contactFilter;
        }

        private static void OnFishMove(FishyMovementComponent component)
        {
            //if this crashes with NullReferenceException, the GameObject probably has no Brain-Component
            component.HandleInput(component);
            ApplyFriction(component);
            Animate(component);
            CalculateCollisions(component);
            ApplyAnimationToModel(component);
            FixCollider(component);
        }

        private static void CalculateCollisions(FishyMovementComponent component)
        {
            var rb2D = component.GetComponent<Rigidbody2D>();

            var deltaPosition = component.Velocity * UnityEngine.Time.fixedDeltaTime;
            var moveAlongGround = new Vector2(component.GroundNormal.y, -component.GroundNormal.x);

            CalculateCollisionAnswer(moveAlongGround * deltaPosition.x, component, rb2D, false);
            CalculateCollisionAnswer(Vector2.up * deltaPosition.y, component, rb2D, false);
        }

        private static void CalculateCollisionAnswer(
            Vector2 movement, FishyMovementComponent component, Rigidbody2D rb2D, bool isVertical)
        {
            var distance = movement.magnitude;

            var hitBuffer = new RaycastHit2D[16];
            var collisionCount = rb2D.Cast(movement, component.ContactFilter, hitBuffer, distance + CollisionShell);
            var hitBufferList = hitBuffer.Take(collisionCount).ToArray();

            if (hitBufferList.Any())
            {
                component.CollisionDetected.Execute(hitBufferList);
            }

            foreach (var hit in hitBufferList)
            {
                var currentNormal = hit.normal;
                if (currentNormal.y > MinGroundNormalY && isVertical)
                {
                    component.GroundNormal = currentNormal;
                    currentNormal.x = 0;
                }

                var projection = Vector2.Dot(component.Velocity, currentNormal);
                if (projection < 0)
                {
                    component.Velocity = component.Velocity - projection * currentNormal;
                }

                var modifiedDistance = hit.distance - CollisionShell;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        private static void Animate(FishyMovementComponent component)
        {
            var nextVelocity = component.Velocity + component.Acceleration * UnityEngine.Time.fixedDeltaTime;

            if (Mathf.Abs(component.Velocity.x) < component.MaxSpeed.x)
            {
                component.Velocity = component.Velocity.ChangeX(nextVelocity.x);
            }
            if (Mathf.Abs(component.Velocity.y) < component.MaxSpeed.y)
            {
                component.Velocity = component.Velocity.ChangeY(nextVelocity.y);
            }
        }

        private static void ApplyFriction(FishyMovementComponent component)
        {
            var backFriction = component.Velocity * -component.BackwardFrictionFactor;
            component.Velocity = component.Velocity + backFriction * UnityEngine.Time.fixedDeltaTime;
        }

        private static void ApplyAnimationToModel(FishyMovementComponent component)
        {
            var positionChange = component.Velocity * UnityEngine.Time.fixedDeltaTime;

            var rb2D = component.GetComponent<Rigidbody2D>();

            rb2D.position = new Vector3(rb2D.position.x + positionChange.x, rb2D.position.y + positionChange.y);
        }

        private static void FixCollider(FishyMovementComponent component)
        {
            if (component.ColliderObject)
            {
                component.ColliderObject.transform.localPosition = Vector3.zero;
            }
        }
    }
}
