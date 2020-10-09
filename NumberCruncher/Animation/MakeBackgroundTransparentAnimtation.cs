using Microsoft.Xna.Framework;
using NumberCruncher.Components;

namespace NumberCruncher.Animation
{
    public class MakeBackgroundTransparentAnimtation : IAnimation
    {
        private double _currentTime;
        private double _maxTime;

        public bool IsRunning { get; set; }

        public MakeBackgroundTransparentAnimtation(double time)
        {
            _maxTime = time;
            _currentTime = 0;
        }

        public bool IsComplete(SadWrapperComponent comp)
        {
            return _currentTime >= _maxTime;
        }

        public void OnEnd(SadWrapperComponent comp)
        {
            comp.AnimateColor(comp.FColor, comp.BColor));
        }

        public void OnStart(GameTime time, SadWrapperComponent comp)
        {
            comp.AnimateColor(comp.FColor, Color.Transparent));
        }

        public void Update(GameTime time, SadWrapperComponent comp)
        {
            _currentTime += time.ElapsedGameTime.TotalMilliseconds;
        }
    }
}
