using SystemBase;
using Systems.Movement;
using UnityEngine;

namespace Systems.Obstacle
{
    [GameSystem(typeof(FishyMovementSystem))]
    public class SimpleFishBrainSystem : GameSystem<SimpleFishBrainComponent>
    {
        public override void Register(SimpleFishBrainComponent component)
        {
            component.GetComponent<FishyMovementComponent>().HandleInput = SimpleFishMovement;
        }

        private void SimpleFishMovement(FishyMovementComponent obj)
        {
            var brain = obj.GetComponent<SimpleFishBrainComponent>();

            var vertical = Mathf.Sin(UnityEngine.Time.realtimeSinceStartup * brain.Multiplier);
            obj.ForwardVector = brain.Direction;
            obj.Acceleration = new Vector2(obj.ForwardVector.x * obj.AccelerationFactor.x, vertical * obj.AccelerationFactor.y);
        }
    }
}
