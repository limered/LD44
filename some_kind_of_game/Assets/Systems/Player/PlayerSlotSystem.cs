using System.Linq;
using SystemBase;
using Systems.Control;
using Systems.Movement.Modifier;
using UniRx;
using UnityEngine;
using Utils.Plugins;

namespace Systems.Player
{
    [GameSystem(typeof(PlayerControlSystem))]
    public class PlayerSlotSystem : GameSystem<PlayerComponent, SlotComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>(null);

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        public override void Register(SlotComponent component)
        {
            //add slots to PlayerComponent (Editor & Game)
            //this is just for usability reasons
            _player.WhereNotNull()
            .Subscribe(player =>
            {
                var slot = player.Slots.Any(x => x.Component == component)
                    ? player.Slots.Find(x => x.Component == component)
                    : new Slot { Component = component };
                player.Slots.Remove(slot);
                player.Slots.Add(slot);
            })
            .AddTo(component);

            //Slot Item selection
            component.SelectedItem
            .Subscribe(name =>
            {
                foreach (var item in component.Arsenal)
                {
                    item.Item.SetActive(item.Name.Trim().ToLower() == name.Trim().ToLower());
                }
            })
            .AddTo(component);
        }
    }
}