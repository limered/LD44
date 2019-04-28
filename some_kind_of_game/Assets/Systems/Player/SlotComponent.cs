using System;
using System.Linq;
using SystemBase;
using Systems.Control;
using UniRx;
using UnityEngine;

namespace Systems.Player
{
    public class SlotComponent : GameComponent
    {
        public string Name = "Enter Name";
        public SlotItem[] Arsenal = new SlotItem[0];

        [Space(30)]
        public StringReactiveProperty SelectedItem = new StringReactiveProperty("");

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