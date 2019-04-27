using System;
using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Animation
{
    public class BasicToggleAnimationComponent : GameComponent
    {
        public const int NotAnimating = -1;
        public float AnimationTime = 1f;
        public GameObject[] Sprites;
        public GameObject EndSprite;
        public bool Reverse;

        #region Helpers
        public IntReactiveProperty SpriteIndexWithoutAnimation { get; } = new IntReactiveProperty(0);

        public int CurrentSprite { get; set; } = NotAnimating;

        public void SetSpriteWithoutAnimation(int index)
        {
            SpriteIndexWithoutAnimation.Value = index;
        }

        public void StartAnimation()
        {
            CurrentSprite = Reverse ? Sprites.Length - 1 : 0;
        }

        public void StopAnimation()
        {
            CurrentSprite = NotAnimating;
        }
        #endregion
    }

}