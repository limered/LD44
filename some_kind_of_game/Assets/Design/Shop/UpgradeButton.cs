using UnityEngine;
using UnityEngine.UI;

namespace Design.Shop
{
    public class UpgradeButton : MonoBehaviour
    {
        public GameObject Thumbnail;

        public void SetThumbnail(Sprite thumbnail)
        {
            var image = Thumbnail.GetComponent<Image>();
            image.sprite = thumbnail;
        }

        public void OnButtonClick()
        {

        }
    }
}
