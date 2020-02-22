using GameState.States;
using SystemBase;
using SystemBase.StateMachineBase;
using Systems.GameState.Messages;
using StrongSystems.Audio.Helper;
using UniRx;
using UnityEngine;
using Utils;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

namespace Systems
{
    public class Game : GameBase
    {
        public readonly StateContext<Game> GameStateContext = new StateContext<Game>();

        [ContextMenu("Start Game")]
        public void StartGame()
        {
            MessageBroker.Default.Publish(new GameMsgStart());
            SceneManager.LoadScene("Level");
        }

        private void Awake()
        {
            IoC.RegisterSingleton(this);

            GameStateContext.Start(new Loading());

            InstantiateSystems();

            Init();

            MessageBroker.Default.Publish(new GameMsgFinishedLoading());

            GameStateContext.CurrentState.Where(state => state is StartScreen).Subscribe(_ => ListenToGameStartButtonPressed());
        }

        public override void Init()
        {
            base.Init();

            IoC.RegisterSingleton<ISFXComparer>(() => new SFXComparer());
        }

        private void ListenToGameStartButtonPressed()
        {
            this.UpdateAsObservable().Subscribe(_ =>
            {
                if (Input.GetButtonDown("Start"))
                {
                    StartGame();
                }
            });
        }
    }
}