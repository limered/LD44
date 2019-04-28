using System;
using System.Linq;
using SystemBase;
using Systems.Control;
using UniRx;
using UnityEngine;

namespace Systems.Player
{
    [ExecuteAlways]
    public class SlotComponent : GameComponent
    {
        public string Name = "Enter Name";
        public SlotItem[] Arsenal = new SlotItem[0];

        [Space(30)]
        public StringReactiveProperty SelectedItem = new StringReactiveProperty("");

        new void Start()
        {
            if (Application.IsPlaying(gameObject))
            {
                base.Start();
            }
            else // Editor logic
            {
                var player = GameObject.FindObjectOfType(typeof(PlayerComponent)) as PlayerComponent;
                if (!player) throw new Exception("no PlayerComponent found in Scene");

                var slot = player.Slots.Any(x => x.Component == this)
                    ? player.Slots.Find(x => x.Component == this)
                    : new Slot { Component = this };
                player.Slots.Remove(slot);
                player.Slots.Add(slot);
            }
        }
    }

    [Serializable]
    public struct Slot
    {
        public SlotComponent Component;
    }

    [Serializable]
    public struct SlotItem
    {
        public string Name;
        public GameObject Item;
    }
}