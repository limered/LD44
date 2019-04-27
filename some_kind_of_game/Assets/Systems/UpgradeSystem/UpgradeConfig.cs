using System;
using UnityEngine;

namespace Systems.UpgradeSystem
{
    [Serializable]
    public class UpgradeConfig
    {
        public UpgradeType UpgradeType;
        public GameObject Thumbnails;
        public int PriceInSeconds;
    }
}