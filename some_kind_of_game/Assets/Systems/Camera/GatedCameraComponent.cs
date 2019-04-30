using SystemBase;
using UnityEngine;

namespace Systems.Camera
{
    public class GatedCameraComponent : GameComponent
    {
        public GameObject Player { get; set; }

        public float AnimationModifier = 1;

        public float RightMovementPoint = -4;
        public float LeftMovementPoint = -1;
    }
}