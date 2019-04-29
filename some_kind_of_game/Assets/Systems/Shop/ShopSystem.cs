using Design.Shop;
using SystemBase;
using Systems.GameState.Messages;
using Systems.Health;
using Systems.Health.Actions;
using Systems.UpgradeSystem;
using StrongSystems.Audio;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        private readonly FloatReactiveProperty _internalHealthValue = new FloatReactiveProperty(0);
        private int ItemsToShow = 0;

        public override void Register(ShopComponent component)
        {
            _shopComponent = component;

            if (_healthComponent.CurrentHealth.Value > 0) ItemsToShow++;
            FinishRegistration();
            MessageBroker.Default.Publish(new HealthActSet
            {
                Value = _internalHealthValue.Value,
                ComponentToChange = _healthComponent,
            });
        }

        public override void Register(UpgradeConfigComponent component)
        {
            _upgradeConfigComponent = component;
            FinishRegistration();
        }

        public override void Register(HealthComponent component)
        {
            _healthComponent = component;
            _internalHealthValue.Value = component.MaxHealth;
            _internalHealthValue.Subscribe(v => MessageBroker.Default.Publish(new HealthActSet
            {
                ComponentToChange = component,
                Value = v,
            }));
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
            _internalHealthValue.Value -= _selectedUpgrade.Value.PriceInSeconds;
            "SellBuy".Play();
        }

        private void SellButtonClicked()
        {
            _selectedUpgrade.Value.IsAdded.Value = false;
            _internalHealthValue.Value += _selectedUpgrade.Value.PriceInSeconds;
            "SellBuy".Play();
        }

        private void ContinueButtonClicked()
        {
            MessageBroker.Default.Publish(new HealthActSet
            {
                ComponentToChange = _healthComponent,
                Value = _internalHealthValue.Value,
            });
            MessageBroker.Default.Publish(new GameMsgUnpause());
            SceneManager.LoadScene("Level");
        }

        private void CreateUpgradeButtons()
        {
            var upgradeConfigs = _upgradeConfigComponent.GetComponent<UpgradeConfigComponent>().UpgradeConfigs;

            var i = 0;
            foreach (var upgradeConfig in upgradeConfigs)
            {
                if (i > ItemsToShow) break;
                i++;

                var upgradeGo = Object.Instantiate(_shopComponent.UpgradeButton, _shopComponent.UpgradeContainer.transform);

                var button = upgradeGo.GetComponent<Button>();
                var upgradeButton = upgradeGo.GetComponent<UpgradeButton>();
                button.OnClickAsObservable()
                    .Subscribe(_ => OnUpgradeClicked(upgradeConfig))
                    .AddTo(button);

                upgradeConfig.IsAdded
                    .Subscribe(_ => OnIsAddedChanged(upgradeButton, upgradeConfig))
                    .AddTo(upgradeGo);

                _selectedUpgrade
                    .Subscribe(_ => OnSelectedUpgradeChanged(upgradeButton, upgradeConfig))
                    .AddTo(upgradeGo);
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
                   _internalHealthValue.Value > _selectedUpgrade.Value.PriceInSeconds;
        }
    }
}
