using System;
using SystemBase;
using SystemBase.StateMachineBase;
using Systems.Control;
using Systems.Movement;
using Systems.Movement.Modifier;
using Systems.Obstacle.JellyFishStates;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Plugins;

namespace Systems.Obstacle
{
    [GameSystem(typeof(FishyMovementSystem))]
    public class JellyfishBrainSystem : GameSystem<PlayerComponent, JellyfishBrainComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>(null);

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        public override void Register(JellyfishBrainComponent component)
        {
            component.GetComponent<FishyMovementComponent>().HandleInput =
            f => JellyFishMovement(f, component);

            //alternating between Standing & Swimming
            component.StateContext.AfterStateChange
            .Where(state => !(state is Adhering))
            .Subscribe(state =>
            {
                Debug.Log("NOT Adhering: " + state.GetType().Name);
                var timeInState = TimeSpan.FromSeconds(
                    state is Standing
                        ? component.StandingTime
                    : state is SwimmingAway
                        ? component.SwimmingAwayTime
                        : component.SwimmingTime
                    );

                var nextState = state is Standing
                    ? (JellyFishState)new Swimming(component)
                    : new Standing(component);

                Observable.Timer(timeInState)
                .Subscribe(_ => { }, () =>
                {
                    component.StateContext.GoToState(nextState);
                })
                .AddTo(state);
            })
            .AddTo(component);

            //adhere to Kailax when trigger is entered
            component.GetCollider()
            .OnTriggerEnter2DAsObservable()
            .Where(c => c.attachedRigidbody.GetComponent<PlayerComponent>())
            .Select(_ => component.StateContext.CurrentState.Value)
            .Where(state => (state is Swimming || state is Standing))
            .Subscribe(_ =>
            {
                component.StateContext.GoToState(new Adhering(component));
            })
            .AddTo(component);

            //adhering by matching player position
            _player.WhereNotNull().Subscribe(player =>
            {
                AccelerationModifier accModifier = null;
                MaxSpeedModifier maxSpeedModifier = null;
                var onlyOne = new SerialDisposable();

                component.StateContext.AfterStateChange
                .Subscribe(state =>
                {
                    if (state is Adhering)
                    {
                        accModifier = player.gameObject.AddComponent<AccelerationModifier>();
                        accModifier.Summand = component.SlowDownAcceleration;

                        maxSpeedModifier = player.gameObject.AddComponent<MaxSpeedModifier>();
                        maxSpeedModifier.Summand = component.SlowDownSpeed;

                        onlyOne.Disposable = component.FixedUpdateAsObservable()
                                        .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(component.AdheringTime)))
                                        .Where(x => component.StateContext.CurrentState.Value is Adhering)
                                        .Subscribe(_ =>
                                        {
                                            component.gameObject.transform.position = player.gameObject.transform.position;
                                        }, () =>
                                        {
                                            component.StateContext.GoToState(new SwimmingAway(component));
                                        });
                    }
                    else
                    {
                        if (accModifier)
                        {
                            GameObject.Destroy(accModifier);
                            accModifier = null;
                        }
                        if (maxSpeedModifier)
                        {
                            GameObject.Destroy(maxSpeedModifier);
                            maxSpeedModifier = null;
                        }
                        onlyOne.Disposable = null;
                    }
                })
                .AddTo(component);
            })
            .AddTo(component);

            component.StateContext.Start(new JellyFishStates.Standing(component));
        }

        private void JellyFishMovement(FishyMovementComponent obj, JellyfishBrainComponent brain)
        {
            var state = brain.StateContext.CurrentState.Value;

            if (state is Swimming || state is SwimmingAway)
            {
                var vertical = Mathf.Sin(UnityEngine.Time.realtimeSinceStartup * brain.Multiplier);
                obj.ForwardVector = brain.Direction;
                obj.Acceleration = new Vector2(obj.ForwardVector.x * obj.AccelerationFactor.x, vertical * obj.AccelerationFactor.y);
            }
            else
            {
                obj.ForwardVector = obj.Acceleration = Vector2.zero;
            }
        }
    }
}
