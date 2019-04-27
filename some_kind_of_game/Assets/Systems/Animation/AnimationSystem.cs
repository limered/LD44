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
    public class AnimationSystem : GameSystem<FinAnimationComponent>
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
    }
}