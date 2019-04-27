using SystemBase;
using Systems.Control;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Plugins;

namespace Systems.Camera
{
    [GameSystem(typeof(PlayerControlSystem))]
    public class CameraSystem : GameSystem<PlayerComponent, GatedCameraComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>(null);

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        public override void Register(GatedCameraComponent gatedCameraComponent)
        {
            _player.WhereNotNull()
                .Select(player => new {player, cameraComponent = gatedCameraComponent})
                .Subscribe(obj => OnSubscribeToPlayer(obj.player, obj.cameraComponent))
                .AddTo(gatedCameraComponent);
        }

        private static void OnSubscribeToPlayer(PlayerComponent player, GatedCameraComponent gatedCamera)
        {
            gatedCamera.Player = player.gameObject;
            gatedCamera.FixedUpdateAsObservable()
                .Select(_ => gatedCamera)
                .Subscribe(AnimateCamera)
                .AddTo(player);
        }

        private static void AnimateCamera(GatedCameraComponent gatedCameraComponent)
        {
            var playerPosition = gatedCameraComponent.Player.transform.position;
            var camPosition = gatedCameraComponent.transform.position;

            var newX = camPosition.x;
            var right = camPosition.x + gatedCameraComponent.RightMovementPoint;
            var left = camPosition.x + gatedCameraComponent.LeftMovementPoint;
            if (playerPosition.x > right)
            {
                var distance = playerPosition.x - right;
                newX = camPosition.x + distance * gatedCameraComponent.AnimationModifier * Time.fixedDeltaTime;
            }
            else if (playerPosition.x < left)
            {
                var distance = playerPosition.x - left;
                newX = camPosition.x + distance * gatedCameraComponent.AnimationModifier * Time.fixedDeltaTime;
            }

            gatedCameraComponent.transform.position = new Vector3(newX,
                gatedCameraComponent.transform.position.y,
                gatedCameraComponent.transform.position.z);
        }
    }
}
