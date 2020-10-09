using Microsoft.Xna.Framework;
using NumberCruncher.Components;
using SadSharp.Helpers;

namespace NumberCruncher.Animation
{
    public class JiggleAnimation : IAnimation
    {
        private double _maxTime;
        private double _currentTime;
        private double _speed;

        public bool IsRunning { get; set; }

        public JiggleAnimation(double maxTime, double speed)
        {
            _maxTime = maxTime;
   
            _speed = speed;
            _currentTime = 0;
        }

        public bool IsComplete(SadWrapperComponent comp)
        {
            return _currentTime >= _maxTime;
        }

        public void OnEnd(SadWrapperComponent comp)
        {
            comp.AnimatePosition(comp.X, comp.Y));
        }

        public void OnStart(GameTime time, SadWrapperComponent comp)
        {
        }

        public void Update(GameTime time, SadWrapperComponent comp)
        {
            var dx = (Roller.NextD3 - 2) * _speed;
            var dy = (Roller.NextD3 - 2) * _speed;
            comp.AnimatePosition(comp.DrawX + dx, comp.DrawY + dy));

            _currentTime += time.ElapsedGameTime.TotalMilliseconds;

        }
    }
}
