using System;
using SystemBase;
using Systems.Animation;
using Systems.Control;
using Systems.Movement;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Obstacle
{
    [GameSystem(typeof(FishyMovementSystem))]
    public class FishBrainSystem : GameSystem<BlowFishBrainComponent, TrashBrainComponent>
    {
        public override void Register(BlowFishBrainComponent component)
        {
            var collider = component.BlowUpCollider;
            var animation = component.GetComponent<BlowFishAnimationComponent>();

            //Grow when in range, then Shrink after some time
            collider.OnTriggerStay2DAsObservable()
                .Where(d => d.attachedRigidbody.GetComponent<PlayerComponent>())
                .Do(_ => animation.Grow())
                .Throttle(TimeSpan.FromSeconds(5))
                .Subscribe(_ =>
                {
                    animation.Shrink();
                })
                .AddTo(component);


            //enable bounce collider
            component.BounceCollider.gameObject.SetActive(false);
            animation.Context.AfterStateChange
            .Subscribe(state =>
            {
                component.BounceCollider.gameObject.SetActive(state is Animation.BlowFishState.Growing);
            })
            .AddTo(component);
        }

        public override void Register(TrashBrainComponent component)
        {
            component.GetComponent<FishyMovementComponent>().HandleInput = TrashyMovement;
        }

        private void TrashyMovement(FishyMovementComponent obj)
        {
            var brain = obj.GetComponent<TrashBrainComponent>();

            var horizontal = Mathf.Sin(UnityEngine.Time.realtimeSinceStartup * brain.Multiplier);
            obj.ForwardVector = brain.Direction;
            obj.Acceleration = new Vector2(horizontal * obj.ForwardVector.x * obj.AccelerationFactor.x, obj.ForwardVector.y * obj.AccelerationFactor.y);
        }
    }
}