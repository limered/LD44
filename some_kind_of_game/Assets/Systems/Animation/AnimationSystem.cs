using System;
using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;

namespace Systems.Animation
{
    [GameSystem]
    public class AnimationSystem : GameSystem<BasicToggleAnimationComponent, FinAnimationComponent, BlowFishAnimationComponent>
    {

        public override void Register(FinAnimationComponent component)
        {
            //initial angle is random
            component.CurrentAngle = UnityEngine.Random.Range(-component.SpreadAngle / 2, component.SpreadAngle / 2);
            component.transform.Rotate(Vector3.up, component.CurrentAngle);

            //rotate the fin between -SpreadAngle/2 and +SpreadAngle/2
            component.FixedUpdateAsObservable()
            .Select(_ => Time.fixedDeltaTime)
            .Subscribe(delta =>
            {
                var angleDelta = ((int)component.Direction * delta * component.WobbleSpeed);

                if (component.Direction == FinDirection.Left)
                {
                    if (component.CurrentAngle + angleDelta <= -component.SpreadAngle / 2)
                    {
                        angleDelta = 0;
                        component.Direction = FinDirection.Right;
                    }

                    component.CurrentAngle = Math.Max(angleDelta + component.CurrentAngle, -component.SpreadAngle / 2);
                }

                if (component.Direction == FinDirection.Right)
                {
                    if (component.CurrentAngle + angleDelta >= component.SpreadAngle / 2)
                    {
                        angleDelta = 0;
                        component.Direction = FinDirection.Left;
                    }

                    component.CurrentAngle = Math.Min(angleDelta + component.CurrentAngle, component.SpreadAngle / 2);
                }

                component.transform.Rotate(Vector3.up, angleDelta);
            })
            .AddTo(component);
        }

        public override void Register(BlowFishAnimationComponent component)
        {

        }

        public override void Register(BasicToggleAnimationComponent component)
        {
            component.FixedUpdateAsObservable()
            .Select(_ => component.CurrentSprite != BasicToggleAnimationComponent.NotAnimating)
            .DistinctUntilChanged()
            .SelectMany(animating => animating ? Observable.FromCoroutine(() => Animate(component)) : Observable.Empty<Unit>())
            .Subscribe()
            .AddTo(component);

            component.SpriteIndexWithoutAnimation
            .Subscribe(index =>
            {
                for (var s = 0; s < component.Sprites.Length; s++)
                {
                    component.Sprites[s].SetActive(s == index);
                }
            })
            .AddTo(component);
        }

        private IEnumerator Animate(BasicToggleAnimationComponent component)
        {
            var steps = component.Sprites.Length;
            var time = component.AnimationTime;
            var delta = time / steps;

            component.StartAnimation();

            for (var i = 0; i < steps; i++)
            {
                for (var s = 0; s < component.Sprites.Length; s++)
                {
                    component.Sprites[s].SetActive(component.CurrentSprite == s);
                }

                yield return new WaitForSeconds(delta);
                component.CurrentSprite += component.Reverse ? -1 : 1;
                component.CurrentSprite = Math.Max(0, component.CurrentSprite);
                component.CurrentSprite = Math.Min(component.CurrentSprite, steps - 1);
            }

            if (component.EndSprite)
            {
                for (var s = 0; s < component.Sprites.Length; s++)
                {
                    component.Sprites[s].SetActive(false);
                }
                component.EndSprite.SetActive(true);
            }

            component.StopAnimation();
        }
    }
}