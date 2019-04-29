using System;
using SystemBase;
using Systems.Control;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Systems.Obstacle
{
    [GameSystem]
    public class FishSpawnSystem : GameSystem<FishSpawnerComponent, IntervalSpawnerComponent>
    {
        public override void Register(FishSpawnerComponent component)
        {
            if (component.TriggerOverwrite)
            {
                component.TriggerOverwrite.OnTriggerEnter2DAsObservable()
                    .Where(d => d.attachedRigidbody.GetComponent<PlayerComponent>())
                    .Subscribe(d => SpawnPrefab(component))
                    .AddTo(component);
            }
            else if (component.Room)
            {
                component.Room.OnEnteredRoom.Subscribe(d => SpawnPrefab(component)).AddTo(component);
            }
            else
            {
                Debug.LogWarning("Spawner can't spawn, no trigger.");
            }
        }

        private void SpawnPrefab(FishSpawnerComponent component)
        {
            var rnd = Random.value * component.SpawnPrefabs.Length;
            Object.Instantiate(
                    component.SpawnPrefabs[(int) rnd], 
                    component.transform.position, 
                    component.transform.rotation, 
                    component.transform);
        }

        public override void Register(IntervalSpawnerComponent component)
        {
            if (component.TriggerOverwrite)
            {
                component.TriggerOverwrite.OnTriggerStay2DAsObservable()
                    .Where(d => d.attachedRigidbody.GetComponent<PlayerComponent>())
                    .Sample(TimeSpan.FromSeconds(component.Interval))
                    .Subscribe(d => SpawnPrefab(component))
                    .AddTo(component);
            }
        }
    }
}