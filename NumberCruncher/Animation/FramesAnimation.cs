using Microsoft.Xna.Framework;
using NumberCruncher.Components;
using System;

namespace NumberCruncher.Animation
{
    public class FramesAnimation : IAnimation
    {
        private int[] _frames;
        private int _timePerFrame;
        private int _numCycles;
        private int _cycleCount;
        private int _currentFrame;
        private double _currentTime;
        private bool _returnToInitialGlyph;

        public bool IsRunning { get; set; }
        public static int RepeatForever = -1;

        public Action<SadWrapperComponent> Callback { get; set; }

        public FramesAnimation(int[] frames, int timePerFrame, int numCycles = 1, bool returnToInitialGlyph = false) 
        {
            _frames = frames;
            _timePerFrame = timePerFrame;
            _numCycles = numCycles;
            _cycleCount = 0;
            _currentFrame = 0;
            _currentTime = 0;

            _returnToInitialGlyph = returnToInitialGlyph;
        }

        public bool IsComplete(SadWrapperComponent comp)
        {
            if (_numCycles == RepeatForever) return false;
            return _cycleCount >= _numCycles; 
        }

        public void OnEnd(SadWrapperComponent comp)
        {
            if(_returnToInitialGlyph)
                comp.AnimateGlyph(comp.GlyphIndex));
        }

        public void OnStart(GameTime time, SadWrapperComponent comp)
        {
            comp.AnimateGlyph(_frames[0]));
        }

        public void Update(GameTime time, SadWrapperComponent comp)
        {
            _currentTime += time.ElapsedGameTime.TotalMilliseconds;

            if (_currentTime > _timePerFrame)
            {
                _currentFrame++;
                _currentTime = 0;
                if (_currentFrame > _frames.Length - 1)
                {
                    _currentFrame = 0;
                    _cycleCount++;
                }
                comp.AnimateGlyph(_frames[_currentFrame]));
            }
        }
    }
}
