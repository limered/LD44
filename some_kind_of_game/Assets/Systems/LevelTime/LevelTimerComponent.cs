using System;
using SystemBase;

namespace Systems.LevelTime
{
    public class LevelTimerComponent : GameComponent
    {
        public float LastBestTime;
        public float CurrentTime;
        public IDisposable TimeObservable { get; set; }
    }
}