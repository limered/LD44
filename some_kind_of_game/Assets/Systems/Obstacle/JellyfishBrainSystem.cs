using SystemBase;
using Systems.Movement;
using UnityEngine;

namespace Systems.Obstacle
{
    [GameSystem(typeof(FishyMovementSystem))]
    public class JellyfishBrainSystem : GameSystem<JellyfishBrainComponent>
    {
        public override void Register(JellyfishBrainComponent component)
        {
            component.GetComponent<FishyMovementComponent>().HandleInput = JellyFishMovement;
        }

        private void JellyFishMovement(FishyMovementComponent obj)
        {
            var brain = obj.GetComponent<JellyfishBrainComponent>();

            var vertical = Mathf.Sin(UnityEngine.Time.realtimeSinceStartup * brain.Multiplier);
            obj.ForwardVector = brain.Direction;
            obj.Acceleration = new Vector2(obj.ForwardVector.x * obj.AccelerationFactor.x, vertical * obj.AccelerationFactor.y);
        }
    }
}
