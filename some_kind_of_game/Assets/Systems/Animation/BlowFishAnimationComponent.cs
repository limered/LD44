using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UnityEngine;

namespace Systems.Animation
{
    public class BlowFishAnimationComponent : GameComponent
    {
        public BlowFishState State;
    }

    public enum BlowFishState
    {
        Swimming,
        Growing,
        Shrinking        
    }
}