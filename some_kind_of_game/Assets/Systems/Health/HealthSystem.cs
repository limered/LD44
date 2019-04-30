using SystemBase;
using Systems.Health.Actions;
using Systems.Health.Events;
using UniRx;

namespace Systems.Health
{
    [GameSystem]
    public class HealthSystem : GameSystem<HealthComponent>
    {
        public override void Register(HealthComponent component)
        {
            component.CurrentHealth.Value = component.MaxHealth;

            MessageBroker.Default.Receive<HealthActSubtract>()
                .Where(subtract => subtract.ComponentToChange == component)
                .Subscribe(subtract => subtract.ComponentToChange.CurrentHealth.Value -= subtract.Value)
                .AddTo(component);

            MessageBroker.Default.Receive<HealthActAdd>()
                .Where(subtract => subtract.ComponentToChange == component)
                .Subscribe(add => add.ComponentToChange.CurrentHealth.Value += add.Value)
                .AddTo(component);

            MessageBroker.Default.Receive<HealthActSet>()
                .Where(subtract => subtract.ComponentToChange == component)
                .Subscribe(set => set.ComponentToChange.CurrentHealth.Value = set.Value)
                .AddTo(component);

            MessageBroker.Default.Receive<HealthActReset>()
                .Where(subtract => subtract.ComponentToChange == component)
                .Subscribe(reset => reset.ComponentToChange.CurrentHealth.Value = reset.ComponentToChange.MaxHealth)
                .AddTo(component);

            component.CurrentHealth
                .Where(f => f <= 0)
                .Subscribe(_ => MessageBroker.Default.Publish(new HealthEvtReachedZero { ObjectToKill = component.gameObject }))
                .AddTo(component);
        }
    }
}