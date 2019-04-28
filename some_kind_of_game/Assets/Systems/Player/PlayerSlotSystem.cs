using System;
using System.Linq;
using SystemBase;
using Systems.Control;
using Systems.Movement.Modifier;
using Systems.UpgradeSystem;
using UniRx;
using UnityEngine;
using Utils.Plugins;

namespace Systems.Player
{
    [GameSystem(typeof(PlayerControlSystem))]
    public class PlayerSlotSystem : GameSystem<PlayerComponent, SlotComponent, UpgradeConfigComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>(null);

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        public override void Register(SlotComponent component)
        {
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

        public override void Register(UpgradeConfigComponent component)
        {
            _player.WhereNotNull()
            .Subscribe(player =>
            {
                foreach (var config in component.UpgradeConfigs)
                {
                    config.IsAdded
                    .Subscribe(added =>
                    {
                        if (added)
                            player.SelectItem(config.UpgradeType.SlotName(), config.UpgradeType.ItemName());
                        else
                            player.SelectItem(config.UpgradeType.SlotName(), "Default");
                    }).AddTo(component);
                }
            })
            .AddTo(component);
        }
    }

    public static class UpgradeSlotMapping
    {
        public static string SlotName(this UpgradeType type)
        {
            switch (type)
            {
                case UpgradeType.Rotor: return "TailFin";
                case UpgradeType.LaserEye: return "Eye";
                default: return "Default";
            }
        }

        public static string ItemName(this UpgradeType type)
        {
            switch (type)
            {
                case UpgradeType.Rotor: return "Rotor";
                case UpgradeType.LaserEye: return "Laser";
                default: return "Default";
            }
        }
    }
}