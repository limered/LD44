using SystemBase;
using Systems.Movement;
using UnityEngine;

namespace Systems.Obstacle
{
    [GameSystem(typeof(FishyMovementSystem))]
    public class ActiveObstacleMovementSystem : GameSystem<WhaleBrainComponent>
    {
        public override void Register(WhaleBrainComponent component)
        {
            component.GetComponent<FishyMovementComponent>().HandleInput = WhaleKi;
        }

        private static void WhaleKi(FishyMovementComponent whaleMovement)
        {
            var whaleComponent = whaleMovement.GetComponent<WhaleBrainComponent>();
            whaleMovement.ForwardVector = whaleComponent.SwimDirection;
            whaleMovement.Acceleration = new Vector2(whaleMovement.ForwardVector.x * whaleComponent.Acceleration, 0);
        }
    }
}
