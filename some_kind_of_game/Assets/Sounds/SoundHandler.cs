using Systems.GameState.Messages;
using Systems.Health.Events;
using StrongSystems.Audio;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sounds
{
    public class SoundHandler : MonoBehaviour
    {
        private void Awake()
        {
            MessageBroker.Default.Receive<GameMsgStart>().Subscribe(_ => OnGameStart()).AddTo(this);
            MessageBroker.Default.Receive<HealthEvtReachedZero>().Subscribe(_ => OnKailaxDeath()).AddTo(this);
        }

        private void OnKailaxDeath()
        {
            "Kailax_Dying".Play();
        }

        private static void OnGameStart()
        {
            "Music".Play();
            "Background".Play();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Shop")
            {
                "YPWYL1".Play();
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
