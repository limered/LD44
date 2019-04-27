using UnityEngine;

namespace Design.Menu
{
    public class QuitOnClick : MonoBehaviour
    {
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

#else
            Application.Quit();
#endif
        }
    }
}
