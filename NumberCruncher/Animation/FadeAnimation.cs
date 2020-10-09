using Microsoft.Xna.Framework;
using NumberCruncher.Components;

namespace NumberCruncher.Animation
{
    public class FadeAnimation : IAnimation
    {
        private int _frames;
        private bool _returnToOriginalColor;
        private float _currentR;
        private float _currentG;
        private float _currentB;

        private float _speedR;
        private float _speedG;
        private float _speedB;

        public bool IsRunning { get; set; }

        public FadeAnimation(int frames, bool returnToOriginalColor = false)
        {
            _frames = frames;
            _returnToOriginalColor = returnToOriginalColor;
        }

        public bool IsComplete(SadWrapperComponent comp)
        {
            return _currentR < 1 && _currentB < 1 && _currentG < 1;
        }

        public void OnEnd(SadWrapperComponent comp)
        {
            if (_returnToOriginalColor) comp.ChangeColor(comp.FColor);
        }

        public void OnStart(GameTime time, SadWrapperComponent comp)
        {
            _currentR = comp.FColor.R;
            _currentB = comp.FColor.B;
            _currentG = comp.FColor.G;

            _speedR = _currentR / (float)_frames;
            _speedB = _currentB / (float)_frames;
            _speedG = _currentG / (float)_frames;
        }

        public void Update(GameTime time, SadWrapperComponent comp)
        {
            _currentR -= _speedR;
            _currentB -= _speedB;
            _currentG -= _speedG;

            var newColor = new Color(_currentR, _currentG, _currentB);

            comp.AnimateColor(newColor);
        }
    }
}
