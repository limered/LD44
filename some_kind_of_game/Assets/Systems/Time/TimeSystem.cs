using SystemBase;
using Systems.Health;
using Systems.Health.Actions;
using GameState.States;
using UniRx;
using UniRx.Triggers;
using Utils;

namespace Systems.Time
{
    [GameSystem(typeof(HealthSystem))]
    public class TimeSystem : GameSystem<HealthComponent>
    {
        public override void Register(HealthComponent component)
        {
            component.UpdateAsObservable()
                .Where(_ => IoC.Game.GameStateContext.CurrentState.Value.GetType() == typeof(Running))
                .Subscribe(_ => MessageBroker.Default.Publish(new HealthActSubtract{ComponentToChange = component, Value = UnityEngine.Time.deltaTime}))
                .AddTo(component);
        }
    }
}
