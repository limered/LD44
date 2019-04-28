using SystemBase;
using Systems.Movement;
using UnityEngine;

namespace Systems.ActiveOstacles
{
    [GameSystem(typeof(FishyMovementSystem))]
    public class ActiveObstacleMovementSystem : GameSystem<WhaleComponent>
    {
        public override void Register(WhaleComponent component)
        {
            component.GetComponent<FishyMovementComponent>().HandleInput = WhaleKi;
        }

        private static void WhaleKi(FishyMovementComponent whaleMovement)
        {
            var whaleComponent = whaleMovement.GetComponent<WhaleComponent>();
            whaleMovement.ForwardVector = whaleComponent.SwimDirection;
            whaleMovement.Acceleration = new Vector2(whaleMovement.ForwardVector.x * whaleComponent.Acceleration, 0);
        }
    }
}
