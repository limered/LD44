using SystemBase;
using Systems.Control;
using UniRx;
using UnityEngine;

namespace Systems.Level
{
    public class RoomComponent : GameComponent
    {
        public Collider2D ActivateCollider;
        public ReactiveCommand OnEnteredRoom = new ReactiveCommand();
    }
}