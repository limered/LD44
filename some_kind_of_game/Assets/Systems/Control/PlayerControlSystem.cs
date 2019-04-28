using SystemBase;
using Systems.Movement;
using GameState.States;
using UnityEngine;
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
            
            component.Acceleration = new Vector2(x, y);
        }
    }
}
