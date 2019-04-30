using SystemBase;
using GameState.States;
using UniRx;
using UniRx.Triggers;
using Utils;

namespace Systems.LevelTime
{
    [GameSystem]
    public class LevelTimerSystem : GameSystem<LevelTimerComponent>
    {
        public override void Register(LevelTimerComponent component)
        {
            component.LastBestTime = float.MaxValue;

            IoC.Game.GameStateContext.CurrentState.Where(state => state is Running)
                .Subscribe(state => StartTimer(component))
                .AddTo(component);

            IoC.Game.GameStateContext.CurrentState.Where(state => state is Paused)
                .Subscribe(state => StopState(component))
                .AddTo(component);
        }

        private void StopState(LevelTimerComponent component)
        {
            component.TimeObservable.Dispose();
            component.LastBestTime = component.CurrentTime < component.LastBestTime 
                ? component.CurrentTime 
                : component.LastBestTime;
            component.CurrentTime = 0;
        }

        private void StartTimer(LevelTimerComponent component)
        {
            component.TimeObservable = component.UpdateAsObservable()
                .Subscribe(unit => component.CurrentTime += UnityEngine.Time.deltaTime);
        }
    }
}
