using System.Linq;
using SystemBase;
using Systems.Control;
using UniRx;
using UniRx.Triggers;

namespace Systems.Level
{
    [GameSystem(typeof(PlayerControlSystem))]
    public class RoomSystem : GameSystem<RoomComponent>
    {
        public override void Register(RoomComponent component)
        {
            component.ActivateCollider.OnTriggerEnter2DAsObservable()
                .Where(d => d.attachedRigidbody.gameObject.GetComponent<PlayerComponent>())
                .Select(d => component)
                .Subscribe(c => c.OnEnteredRoom.Execute())
                .AddTo(component);
        }
    }
}