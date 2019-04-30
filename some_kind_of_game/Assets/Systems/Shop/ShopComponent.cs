using SystemBase;
using UnityEngine;

namespace Systems.Shop
{
    [RequireComponent(typeof(Canvas))]
    public class ShopComponent : GameComponent
    {
        public GameObject UpgradeContainer;
        public GameObject UpgradeButton;
        public GameObject BuyButton;
        public GameObject SellButton;
        public GameObject ContinueButton;
        public GameObject BestTimeField;
    }
}
