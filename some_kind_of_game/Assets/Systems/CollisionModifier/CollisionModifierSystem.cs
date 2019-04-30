using System;
using SystemBase;
using Systems.Control;
using Systems.Movement.Modifier;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.CollisionModifier
{
    [GameSystem]
    public class CollisionModifierSystem : GameSystem<CollisionModifierComponent>
    {
        public override void Register(CollisionModifierComponent component)
        {
            component.CollisionTrigger.OnTriggerEnter2DAsObservable()
                .Where(d => d.attachedRigidbody.GetComponent<PlayerComponent>())
                .Select(d => new { player = d.attachedRigidbody, modifier = component })
                .Subscribe(t => AttachModifiers(t.player, t.modifier))
                .AddTo(component);
        }

        private void AttachModifiers(Rigidbody2D playerBody, CollisionModifierComponent modifier)
        {
            foreach (var mod in modifier.Modifiers)
            {
                var modAdded = AddComponentToPlayer(mod.Type, mod.SummandVector, playerBody.gameObject, out var addedMod);
                if (!modAdded)
                {
                    modAdded = AddComponentToPlayer(mod.Type, mod.SummandValue, playerBody.gameObject, out addedMod);
                }
                if (!modAdded)
                {
                    Debug.LogError("Something is wrong. Not Implemented Modifier Type?");
                }
                modifier.ActiveModifiers.Add(addedMod);

                if (mod.isTimed)
                {
                    Observable
                        .Timer(TimeSpan.FromSeconds(mod.Duration))
                        .Subscribe(l => { modifier.ActiveModifiers.ForEach(UnityEngine.Object.Destroy); });
                }
                else
                {
                    modifier.CollisionTrigger.OnTriggerExit2DAsObservable()
                        .Take(1)
                        .Subscribe(d => modifier.ActiveModifiers.ForEach(UnityEngine.Object.Destroy));
                }
            }
        }

        public bool AddComponentToPlayer(ModifierType type, Vector2 summand, GameObject player, out GameComponent modifier)
        {
            switch (type)
            {
                case ModifierType.AccelerationFactor:
                    {
                        var mod = player.AddComponent<AccelerationModifier>();
                        mod.Summand = summand;
                        modifier = mod;
                        return true;
                    }
                case ModifierType.MaxSpeed:
                    {
                        var mod = player.AddComponent<MaxSpeedModifier>();
                        mod.Summand = summand;
                        modifier = mod;
                        return true;
                    }
                case ModifierType.Friction:
                    break;
            }

            modifier = null;
            return false;
        }

        public bool AddComponentToPlayer(ModifierType type, float summand, GameObject player, out GameComponent modifier)
        {
            switch (type)
            {
                case ModifierType.AccelerationFactor:
                    break;

                case ModifierType.MaxSpeed:
                    break;

                case ModifierType.Friction:
                    {
                        var mod = player.AddComponent<FrictionModifier>();
                        mod.Summand = summand;
                        modifier = mod;
                        return true;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            modifier = null;
            return false;
        }
    }
}
