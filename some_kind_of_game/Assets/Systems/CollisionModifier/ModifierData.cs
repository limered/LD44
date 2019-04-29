using System;
using UnityEngine;

namespace Systems.CollisionModifier
{
    [Serializable]
    public class ModifierData
    {
        public ModifierType Type;
        public float SummandValue;
        public Vector2 SummandVector;
        public bool isTimed;
        public float Duration;
    }
}