using System.Collections;
using System.Collections.Generic;
using SystemBase;
using SystemBase.StateMachineBase;
using UniRx;
using UnityEngine;

namespace Systems.Animation
{
    public class BlowFishAnimationComponent : GameComponent
    {
        public readonly StateContext<BlowFishAnimationComponent> Context = new StateContext<BlowFishAnimationComponent>();
        public BasicToggleAnimationComponent GrowAnimation;
        public BasicToggleAnimationComponent ShrinkAnimation;

        public void Grow()
        {
            Context.GoToState(new BlowFishState.Growing(this));
        }

        public void Shrink()
        {
            Context.GoToState(new BlowFishState.Shrinking(this));
        }
    }

    namespace BlowFishState
    {
        public abstract class BaseBlowFishState : BaseState<BlowFishAnimationComponent>
        {
            public readonly BlowFishAnimationComponent BlowFish;
            protected BaseBlowFishState(BlowFishAnimationComponent comp)
            {
                BlowFish = comp;
            }

            protected void StopAnimations()
            {
                BlowFish.ShrinkAnimation.StopAnimation();
                BlowFish.GrowAnimation.StopAnimation();
            }
        }

        [NextValidStates(typeof(Growing))]
        public class Swimming : BaseBlowFishState
        {
            public Swimming(BlowFishAnimationComponent comp) : base(comp) { }

            public override void Enter(StateContext<BlowFishAnimationComponent> context)
            {

                BlowFish.GrowAnimation.SetSpriteWithoutAnimation(0);
            }
        }

        [NextValidStates(typeof(Shrinking))]
        public class Growing : BaseBlowFishState
        {
            public Growing(BlowFishAnimationComponent comp) : base(comp) { }

            public override void Enter(StateContext<BlowFishAnimationComponent> context)
            {
                StopAnimations();
                BlowFish.GrowAnimation.StartAnimation();
            }
        }

        [NextValidStates(typeof(Swimming), typeof(Growing))]
        public class Shrinking : BaseBlowFishState
        {
            public Shrinking(BlowFishAnimationComponent comp) : base(comp) { }

            public override void Enter(StateContext<BlowFishAnimationComponent> context)
            {
                StopAnimations();
                BlowFish.ShrinkAnimation.StartAnimation();
            }
        }
    }

}