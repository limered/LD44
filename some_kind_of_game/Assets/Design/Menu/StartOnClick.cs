using GameState.States;
using UniRx;
using UnityEngine;

namespace Design.Menu
{
    public class StartOnClick : MonoBehaviour
    {
        public void StartGame()
        {
            MessageBroker.Default.Publish(new Running());
        }
    }
}
