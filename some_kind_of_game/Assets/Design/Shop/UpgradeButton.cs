using UnityEngine;
using UnityEngine.UI;

namespace Design.Shop
{
    public class UpgradeButton : MonoBehaviour
    {
        public GameObject Thumbnail;
        public GameObject Border;

        public void SetThumbnail(Sprite thumbnail)
        {
            var thumbnailImage = Thumbnail.GetComponent<Image>();
            thumbnailImage.sprite = thumbnail;
        }

        public void SetSelected(bool isSelected)
        {
            var borderImage = Border.GetComponent<Image>();
            borderImage.enabled = isSelected;
        }
    }
}
