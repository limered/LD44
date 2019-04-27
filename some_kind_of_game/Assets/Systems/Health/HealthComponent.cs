using SystemBase;
using UniRx;

namespace Systems.Health
{
    public class HealthComponent : GameComponent
    {
        public float MaxHealth = 100;
        public FloatReactiveProperty CurrentHealth = new FloatReactiveProperty(0);
    }
}