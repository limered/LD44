using Design.Shop;
using SystemBase;
using Systems.Health;
using Systems.Health.Actions;
using Systems.UpgradeSystem;
using GameState.States;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Shop
{
    [GameSystem(typeof(HealthSystem))]
    public class ShopSystem : GameSystem<ShopComponent, UpgradeConfigComponent, HealthComponent>
    {
        private ShopComponent _shopComponent;
        private UpgradeConfigComponent _upgradeConfigComponent;
        private readonly ReactiveProperty<UpgradeConfig> _selectedUpgrade = new ReactiveProperty<UpgradeConfig>(null);
        private Image _selectedThumbnail;
        private Button _buyButton;
        private Button _sellButton;
        private Button _continueButton;
        private HealthComponent _healthComponent;

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

        public override void Register(HealthComponent component)
        {
            _healthComponent = component;
        }

        private void FinishRegistration()
        {
            if (_shopComponent == null || _upgradeConfigComponent == null)
            {
                return;
            }

            InitButtons();
            CreateUpgradeButtons();
            InitSelectedUpgrade();
        }

        private void InitSelectedUpgrade()
        {
            _selectedUpgrade.AsObservable().Subscribe(_ => OnSelectedUpgradeChanged()).AddTo(_shopComponent);
        }

        private void InitButtons()
        {
            _buyButton = _shopComponent.BuyButton.GetComponent<Button>();
            _buyButton.OnClickAsObservable().Subscribe(_ => BuyButtonClicked()).AddTo(_buyButton);

            _sellButton = _shopComponent.SellButton.GetComponent<Button>();
            _sellButton.OnClickAsObservable().Subscribe(_ => SellButtonClicked()).AddTo(_sellButton);

            _continueButton = _shopComponent.ContinueButton.GetComponent<Button>();
            _continueButton.OnClickAsObservable().Subscribe(_ => ContinueButtonClicked()).AddTo(_continueButton);
        }

        private void BuyButtonClicked()
        {
            _selectedUpgrade.Value.IsAdded.Value = true;
            MessageBroker.Default.Publish(new HealthActSubtract
                {ComponentToChange = _healthComponent, Value = _selectedUpgrade.Value.PriceInSeconds});
        }

        private void SellButtonClicked()
        {
            _selectedUpgrade.Value.IsAdded.Value = false;
            MessageBroker.Default.Publish(new HealthActAdd
                {ComponentToChange = _healthComponent, Value = _selectedUpgrade.Value.PriceInSeconds});
        }

        private static void ContinueButtonClicked()
        {
            MessageBroker.Default.Publish(new Running());
        }

        private void CreateUpgradeButtons()
        {
            var upgradeConfigs = _upgradeConfigComponent.GetComponent<UpgradeConfigComponent>().UpgradeConfigs;
            foreach (var upgradeConfig in upgradeConfigs)
            {
                var upgradeGo = Object.Instantiate(_shopComponent.UpgradeButton, _shopComponent.UpgradeContainer.transform);

                var button = upgradeGo.GetComponent<Button>();
                var upgradeButton = upgradeGo.GetComponent<UpgradeButton>();
                button.OnClickAsObservable().Subscribe(_ => OnUpgradeClicked(upgradeConfig)).AddTo(button);

                upgradeConfig.IsAdded.AsObservable().Subscribe(_ => OnIsAddedChanged(upgradeButton, upgradeConfig))
                    .AddTo(_upgradeConfigComponent);

                _selectedUpgrade.AsObservable().Subscribe(_ => OnSelectedUpgradeChanged(upgradeButton, upgradeConfig)).AddTo(_shopComponent);
            }
        }

        private void OnSelectedUpgradeChanged(UpgradeButton upgradeButton, UpgradeConfig upgradeConfig)
        {
           upgradeButton.Border.GetComponent<Image>().enabled = _selectedUpgrade.Value == upgradeConfig;
        }

        private void OnIsAddedChanged(UpgradeButton upgradeButton, UpgradeConfig upgradeConfig)
        {
            UpdateThumbnail(upgradeButton, upgradeConfig);
            OnSelectedUpgradeChanged();
        }

        private static void UpdateThumbnail(UpgradeButton upgradeButton, UpgradeConfig upgradeConfig)
        {
            var thumbnail = upgradeConfig.IsAdded.Value
                ? upgradeConfig.ActiveThumbnail
                : upgradeConfig.InactiveThumbnail;
            upgradeButton.SetThumbnail(thumbnail);
        }

        private void OnUpgradeClicked(UpgradeConfig upgradeConfig)
        {
            _selectedUpgrade.Value = _selectedUpgrade.Value == upgradeConfig ? null : upgradeConfig;
        }

        private void OnSelectedUpgradeChanged()
        {
            if (_selectedUpgrade.Value == null)
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
            return _selectedUpgrade.Value.IsAdded.Value;
        }

        private bool CanBuyUpgrade()
        {
            return !_selectedUpgrade.Value.IsAdded.Value &&
                   _healthComponent.CurrentHealth.Value > _selectedUpgrade.Value.PriceInSeconds;
        }
    }
}
