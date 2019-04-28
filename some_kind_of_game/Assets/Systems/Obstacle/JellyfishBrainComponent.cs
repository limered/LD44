using System;
using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx;
using UnityEngine;
using Systems.Animation;
using SystemBase.StateMachineBase;

namespace Systems.Obstacle
{
    public class JellyfishBrainComponent : GameComponent
    {
        public float Multiplier = 1;
        public Vector2 Direction = Vector2.left;
    }

    namespace JellyFishStates
    {
        public abstract class JellyFishState : BaseState<JellyfishBrainComponent>
        {
            private JellyfishBrainComponent Jellyfish;

            public JellyFishState(JellyfishBrainComponent component)
            {
                Jellyfish = component;
            }
        }

        [NextValidStates(typeof(Swimming), typeof(Adhering))]
        public class Standing : JellyFishState
        {
            public Standing(JellyfishBrainComponent component) : base(component) { }

            public override void Enter(StateContext<JellyfishBrainComponent> context)
            {
            }
        }

        [NextValidStates(typeof(Standing), typeof(Adhering))]
        public class Swimming : JellyFishState
        {
            public Swimming(JellyfishBrainComponent component) : base(component) { }
            public override void Enter(StateContext<JellyfishBrainComponent> context)
            {
            }
        }

        [NextValidStates(typeof(Swimming), typeof(Standing))]
        public class Adhering : JellyFishState
        {
            public Adhering(JellyfishBrainComponent component) : base(component) { }
            public override void Enter(StateContext<JellyfishBrainComponent> context)
            {
            }
        }
    }

}