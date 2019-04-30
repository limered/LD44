using System.Collections.Generic;
using System.Linq;
using SystemBase;
using Systems.Movement;
using Systems.Player;
using UnityEngine;

namespace Systems.Control
{
    [RequireComponent(typeof(FishyMovementComponent))]
    public class PlayerComponent : GameComponent
    {
        [Header("Context Menu of Component -> Find Slots")]
        public List<Player.Slot> Slots = new List<Player.Slot>();
        public void SelectItem(string slotName, string itemName)
        {
            if (Slots.Any(x => x.Component.Name.Trim().ToLower() == slotName.Trim().ToLower()))
            {
                var slot = Slots.Find(x => x.Component.Name.Trim().ToLower() == slotName.Trim().ToLower());
                slot.Component.SelectedItem.Value = itemName;
            }
        }

        [ContextMenu("Find Slots")]
        public void FindSlots()
        {
            var slotComponents = gameObject.GetComponentsInChildren<SlotComponent>();
            Slots.Clear();

            foreach (var component in slotComponents)
            {
                var slot = Slots.Any(x => x.Component == component)
                    ? Slots.Find(x => x.Component == component)
                    : new Slot { Component = component };
                Slots.Add(slot);
            }
        }
    }
}