using System;
using SystemBase;
using Systems.Health;
using UniRx;
using UnityEngine.UI;

namespace Systems.HealthBar
{
    [GameSystem]
    public class HealthBarSystem : GameSystem<HealthBarComponent, HealthComponent>
    {
        private HealthBarComponent _healthBarComponent;
        private HealthComponent _healthComponent;

        public override void Register(HealthBarComponent component)
        {
            _healthBarComponent = component;
            FinishRegistration();
        }

        public override void Register(HealthComponent component)
        {
            _healthComponent = component;
            FinishRegistration();
        }

        private void FinishRegistration()
        {
            if (_healthComponent == null || _healthBarComponent == null)
            {
                return;
            }

            _healthBarComponent.GetComponent<Slider>().maxValue = _healthComponent.MaxHealth;
            _healthComponent.CurrentHealth.AsObservable().Where(_ => _healthBarComponent != null).Subscribe(OnHealthChanged)
                .AddTo(_healthComponent);
        }

        private void OnHealthChanged(float newHealth)
        {
            _healthBarComponent.GetComponent<Slider>().value = newHealth;
        }
    }
}
