using UnityEngine;

namespace Shaders.ImageProcessing
{
    public class Wobble : MonoBehaviour
    {
        public Material RenderMaterial;

        private RenderTexture _renderTexture;

        private void OnEnable()
        {
            var camera = GetComponent<Camera>();
            const int height = 540;
            const int width = 960;
            _renderTexture = new RenderTexture(width, height, 16) { filterMode = FilterMode.Point };
        }

        private void OnDisable()
        {
            Destroy(_renderTexture);
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, RenderMaterial);
        }
    }
}