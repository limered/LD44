using System;
using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx;
using UnityEngine;
using Systems.Animation;
using SystemBase.StateMachineBase;
using UniRx.Triggers;
using Systems.Movement;

namespace Systems.Obstacle
{
    [RequireComponent(typeof(FishyMovementComponent))]
    public class JellyfishBrainComponent : GameComponent
    {
        public float Multiplier = 1;
        public Vector2 Direction = Vector2.left;

        public float StandingTime = 2f;
        public float SwimmingTime = 6f;
        public float SwimmingAwayTime = 3f;
        public float AdheringTime = 2f;

        public GameObject Standing;
        public GameObject Swimming;
        public GameObject Adhering;

        [Header("can be set manually, otherwise fetched from children")]
        public Collider2D Collider;
        public Collider2D GetCollider()
        {
            return Collider ?? gameObject.GetComponentInChildren<Collider2D>();
        }

        public StateContext<JellyfishBrainComponent> StateContext = new StateContext<JellyfishBrainComponent>();
    }

    namespace JellyFishStates
    {
        public abstract class JellyFishState : BaseState<JellyfishBrainComponent>
        {
            protected readonly JellyfishBrainComponent Jellyfish;

            public JellyFishState(JellyfishBrainComponent component)
            {
                Jellyfish = component;
            }

            public override void Enter(StateContext<JellyfishBrainComponent> context)
            {
                ActivateMode();
            }

            protected void ActivateMode()
            {
                Debug.Log(this);
                Jellyfish.Standing.SetActive(this is Standing);
                Jellyfish.Swimming.SetActive(this is Swimming || this is SwimmingAway);
                Jellyfish.Adhering.SetActive(this is Adhering);
            }
        }

        [NextValidStates(typeof(Swimming), typeof(Adhering))]
        public class Standing : JellyFishState
        {
            public Standing(JellyfishBrainComponent component) : base(component) { }
        }

        [NextValidStates(typeof(Standing), typeof(Adhering))]
        public class Swimming : JellyFishState
        {
            public Swimming(JellyfishBrainComponent component) : base(component)
            {
            }
        }

        [NextValidStates(typeof(Swimming), typeof(Standing))]
        public class SwimmingAway : JellyFishState
        {
            public SwimmingAway(JellyfishBrainComponent component) : base(component)
            {
            }
        }

        [NextValidStates(typeof(SwimmingAway))]
        public class Adhering : JellyFishState
        {
            public Adhering(JellyfishBrainComponent component) : base(component) { }
        }
    }

}