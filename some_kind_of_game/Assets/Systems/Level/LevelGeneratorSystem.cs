using SystemBase;
using UnityEngine;

namespace Systems.Level
{
    [GameSystem]
    public class LevelGeneratorSystem : GameSystem<LevelSpawner>
    {
        private int _partlength = 64;

        public override void Register(LevelSpawner component)
        {
            for (var i = 0; i < component.PartsPerLevel; i++)
            {
                var rnd = Random.value * component.RoomPartPrefabs.Count;
                Object.Instantiate(component.RoomPartPrefabs[(int) rnd], new Vector3(i* _partlength, 0, 0), Quaternion.identity,
                    component.transform);
            }
        }
    }
}
