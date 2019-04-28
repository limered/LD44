using System;
using UnityEngine;

namespace Systems.UpgradeSystem
{
    [Serializable]
    public class UpgradeConfig
    {
        public UpgradeType UpgradeType;
        public Sprite ActiveThumbnail;
        public Sprite InactiveThumbnail;
        public int PriceInSeconds;
        public bool IsAdded;
    }
}