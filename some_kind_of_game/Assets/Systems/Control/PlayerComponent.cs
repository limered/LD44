using System.Collections.Generic;
using System.Linq;
using SystemBase;
using Systems.Movement;
using UnityEngine;

namespace Systems.Control
{
    [RequireComponent(typeof(FishyMovementComponent))]
    public class PlayerComponent : GameComponent
    {
        public List<Player.Slot> Slots = new List<Player.Slot>();
        public void SelectItem(string slotName, string itemName)
        {
            if (Slots.Any(x => x.Component.Name.Trim().ToLower() == slotName.Trim().ToLower()))
            {
                var slot = Slots.Find(x => x.Component.Name.Trim().ToLower() == slotName.Trim().ToLower());
                slot.Component.SelectedItem.Value = itemName;
            }
        }
    }
}