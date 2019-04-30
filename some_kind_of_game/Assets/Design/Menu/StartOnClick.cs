using Systems.GameState.Messages;
using GameState.States;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Design.Menu
{
    public class StartOnClick : MonoBehaviour
    {
        public void StartGame()
        {
            MessageBroker.Default.Publish(new GameMsgStart());
            SceneManager.LoadScene("Level");
        }
    }
}
