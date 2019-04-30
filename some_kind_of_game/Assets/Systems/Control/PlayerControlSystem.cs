using GameState.States;
using System;
using SystemBase;
using Systems.GameState.Messages;
using Systems.Health.Events;
using Systems.Movement;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Utils.Unity;

namespace Systems.Control
{
    [GameSystem()]
    public class PlayerControlSystem : GameSystem<PlayerComponent>
    {
        public override void Register(PlayerComponent component)
        {
            component.GetComponent<FishyMovementComponent>().HandleInput = HandlePlayerInput;

            MessageBroker.Default.Receive<HealthEvtReachedZero>()
                .Subscribe(zero => EndGame(component))
                .AddTo(component);
        }

        private void EndGame(PlayerComponent component)
        {
            MessageBroker.Default.Publish(new GameMsgPause());
            component.GetComponent<Rigidbody2D>().rotation = 180;

            component.SelectItem("TailFin", "DeadTailFin");
            component.SelectItem("Eye", "DeadEye");

            component.GetComponent<FishyMovementComponent>().HandleInput = HandlePlayerDeadInput;

            Observable.Timer(TimeSpan.FromSeconds(3))
                .Take(1)
                .Subscribe(l =>
                {
                    SceneManager.LoadScene("Shop");
                });
        }

        private void HandlePlayerDeadInput(FishyMovementComponent obj)
        {
            obj.Acceleration = new Vector2(0, 5);
        }

        private static void HandlePlayerInput(FishyMovementComponent component)
        {
            if (IoC.Game.GameStateContext.CurrentState.Value.GetType() != typeof(Running))
            {
                component.Acceleration = Vector2.zero;
                return;
            }

            float x = 0;
            float y = 0;
            if (KeyCode.D.IsPressed() || KeyCode.RightArrow.IsPressed())
            {
                component.ForwardVector = Vector2.right;
                x = component.ForwardVector.x * component.AccelerationFactor.x;
            }
            else if (KeyCode.A.IsPressed() || KeyCode.LeftArrow.IsPressed())
            {
                component.ForwardVector = Vector2.left;
                x = component.ForwardVector.x * component.AccelerationFactor.x;
            }

            if (KeyCode.W.IsPressed() || KeyCode.UpArrow.IsPressed())
            {
                y = Vector2.up.y * component.AccelerationFactor.y;
            }
            else if (KeyCode.S.IsPressed() || KeyCode.DownArrow.IsPressed())
            {
                y = Vector2.down.y * component.AccelerationFactor.y;
            }

            if (component.transform.position.y > 8 && y > 0)
            {
                y = -y;
            }

            component.Acceleration = new Vector2(x, y);
        }
    }
}
