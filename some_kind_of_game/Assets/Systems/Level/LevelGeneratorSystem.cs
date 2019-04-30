using SystemBase;
using Systems.Control;
using Systems.GameState.Messages;
using GameState.States;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Systems.Level
{
    [GameSystem]
    public class LevelGeneratorSystem : GameSystem<LevelSpawner, PlayerSpawnComponent, FinishComponent>
    {
        private int _partlength = 64;
        private int _spawnWidth = 12;

        public override void Register(LevelSpawner component)
        {
            Object.Instantiate(component.PlayerSpawnPrefab, new Vector3(-22, 0, 0), Quaternion.identity,
                component.transform);
            for (var i = 0; i < component.PartsPerLevel; i++)
            {
                var rnd = Random.value * component.RoomPartPrefabs.Count;
                Object.Instantiate(component.RoomPartPrefabs[(int)rnd], new Vector3(i * _partlength, 0, 0), Quaternion.identity,
                    component.transform);
            }
            Object.Instantiate(component.FinishPrefab, new Vector3(component.PartsPerLevel * _partlength + 5, 0, 0), Quaternion.identity,
                component.transform);
        }

        public override void Register(PlayerSpawnComponent component)
        {
            Object.Instantiate(component.PlayerPrefab, component.transform.position, component.transform.rotation);
        }

        public override void Register(FinishComponent component)
        {
            component.ExitTrigger.OnTriggerEnter2DAsObservable()
                .Where(d => d.attachedRigidbody.GetComponent<PlayerComponent>())
                .Subscribe(d =>
                {
                    MessageBroker.Default.Publish(new GameMsgPause());
                    SceneManager.LoadScene("Shop");
                })
                .AddTo(component);
        }
    }
}