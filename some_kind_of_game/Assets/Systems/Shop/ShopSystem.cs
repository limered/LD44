using System.Collections.Generic;
using System.Linq;
using SystemBase;
using Systems.UpgradeSystem;
using Design.Shop;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Systems.Shop
{
    [GameSystem]
    public class ShopSystem : GameSystem<ShopComponent, UpgradeConfigComponent>
    {
        private ShopComponent _shopComponent;
        private UpgradeConfigComponent _upgradeConfigComponent;
        private UpgradeConfig _selectedUpgrade;
        private Button _buyButton;
        private Button _sellButton;

        public override void Register(ShopComponent component)
        {
            _shopComponent = component;
            FinishRegistration();
        }

        public override void Register(UpgradeConfigComponent component)
        {
            _upgradeConfigComponent = component;
            FinishRegistration();
        }

        private void FinishRegistration()
        {
            if (_shopComponent == null || _upgradeConfigComponent == null)
            {
                return;
            }

            CreateUpgradeButtons();
            InitBuyAndSellButtons();
        }

        private void InitBuyAndSellButtons()
        {
            _buyButton = _shopComponent.BuyButton.GetComponent<Button>();
            _sellButton = _shopComponent.SellButton.GetComponent<Button>();
            UpdateButtonStates();
        }

        private void CreateUpgradeButtons()
        {
            var upgradeConfigs = _upgradeConfigComponent.GetComponent<UpgradeConfigComponent>().UpgradeConfigs;
            foreach (var upgradeConfig in upgradeConfigs)
            {
                var upgradeGo = Game.Instantiate(_shopComponent.UpgradeButton, _shopComponent.UpgradeContainer.transform);
                SetThumbnail(upgradeGo, upgradeConfig);
                var component = upgradeGo.GetComponent<Button>();
                component.OnClickAsObservable().Subscribe(_ => OnUpgradeClicked(upgradeConfig)).AddTo(component);
            }
        }

        private void OnUpgradeClicked(UpgradeConfig upgradeConfig)
        {
            if (_selectedUpgrade == upgradeConfig)
            {
                _selectedUpgrade = null;
            }
            else
            {
                _selectedUpgrade = upgradeConfig;
            }
            
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            if (_selectedUpgrade == null)
            {
                _buyButton.interactable = false;
                _sellButton.interactable = false;
            }
            else
            {
                _buyButton.interactable = CanBuyUpgrade();
                _sellButton.interactable = CanSellUpgrade();
            }
        }

        private bool CanSellUpgrade()
        {
            return _selectedUpgrade.IsAdded;
        }

        private bool CanBuyUpgrade()
        {
            return !_selectedUpgrade.IsAdded;
        }

        private static void SetThumbnail(GameObject upgradeGo, UpgradeConfig upgradeConfig)
        {
            var thumbnail = upgradeConfig.IsAdded ? upgradeConfig.ActiveThumbnail : upgradeConfig.InactiveThumbnail;
            upgradeGo.GetComponent<UpgradeButton>().SetThumbnail(thumbnail);
        }

        public override void Init()
        {
            //component.UpdateAsObservable().Select(_ => new { upgradeConfigs, canvas })
            //    .Subscribe(x => OnShopUpdate(x.upgradeConfigs, x.canvas)).AddTo(component);
        }

        private static void OnShopUpdate(List<UpgradeConfig> upgrades, Canvas canvas)
        {
            var canvasTransform = canvas.gameObject.transform;

            var upgrade = upgrades.First();

            
        }
    }
}
