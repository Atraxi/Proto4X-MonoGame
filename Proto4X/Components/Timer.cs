using MonoGameLibrary.Components;

namespace Proto4X.Components
{
    public struct Timer(float duration, bool shouldRepeat)
    {
        public float Duration { get; set; } = duration;
        public float Remaining { get; set; } = duration;
        public bool ShouldRepeat { get; set; } = shouldRepeat;
        public bool HasJustElapsed { get; set; } = false;
    }
    public interface ITimerProvider : IComponentProvider
    {
        public Timer[] Timers { get; }
    }
}
