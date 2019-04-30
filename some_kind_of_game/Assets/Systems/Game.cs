﻿using GameState.States;
using SystemBase;
using SystemBase.StateMachineBase;
using Systems.GameState.Messages;
using StrongSystems.Audio;
using StrongSystems.Audio.Helper;
using UniRx;
using UnityEngine;
using Utils;

namespace Systems
{
    public class Game : GameBase
    {
        public readonly StateContext<Game> GameStateContext = new StateContext<Game>();

        [ContextMenu("Start Game")]
        public void StartGame()
        {
            MessageBroker.Default.Publish(new GameMsgStart());
        }

        private void Awake()
        {
            IoC.RegisterSingleton(this);

            GameStateContext.Start(new Loading());

            InstantiateSystems();

            Init();

            MessageBroker.Default.Publish(new GameMsgFinishedLoading());
        }

        public override void Init()
        {
            base.Init();

            IoC.RegisterSingleton<ISFXComparer>(() => new SFXComparer());
        }
    }
}