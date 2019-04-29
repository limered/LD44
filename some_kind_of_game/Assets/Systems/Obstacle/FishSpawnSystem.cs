using SystemBase;
using Systems.Control;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Obstacle
{
    [GameSystem]
    public class FishSpawnSystem : GameSystem<FishSpawnerComponent>
    {
        public override void Register(FishSpawnerComponent component)
        {
            if (component.TriggerOverwrite)
            {
                component.TriggerOverwrite.OnTriggerEnter2DAsObservable()
                    .Where(d => d.attachedRigidbody.GetComponent<PlayerComponent>())
                    .Subscribe(d => SpawnFish(component))
                    .AddTo(component);
            }
            else if (component.Room)
            {
                component.Room.OnEnteredRoom.Subscribe(d => SpawnFish(component)).AddTo(component);
            }
            else
            {
                Debug.LogWarning("Spawner can't spawn, no trigger.");
            }
        }

        private void SpawnFish(FishSpawnerComponent component)
        {
            Object.Instantiate(component.SopawnPrefab, component.transform.position, component.transform.rotation, component.transform);
        }
    }
}