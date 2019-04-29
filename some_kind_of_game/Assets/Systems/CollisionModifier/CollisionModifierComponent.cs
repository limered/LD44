using System.Collections.Generic;
using SystemBase;
using UnityEngine;

namespace Systems.CollisionModifier
{
    public class CollisionModifierComponent : GameComponent
    {
        public Collider2D CollisionTrigger;
        public ModifierData[] Modifiers;
        public List<GameComponent> ActiveModifiers;
    }
}