using Microsoft.Xna.Framework;
using NumberCruncher.Components;
using System;

namespace ReferenceGame.Modes.Entity
{
    public class HopAnimation : IAnimation
    {
        private int _numHops;
        private int _maxHops;
        private int _framesPerHop;
        private int _currentFrame;
        private bool _hopping;
        private double _magnitude;

        public bool IsRunning { get; set; }

        public HopAnimation(int maxHops, int framesPerHop, double magnitude)
        {
            _maxHops = maxHops;
            _framesPerHop = framesPerHop;
            _currentFrame = 0;
            _hopping = false;
            _magnitude = magnitude;
        }

        public bool IsComplete(SadWrapperComponent comp)
        {
            return _numHops >= _maxHops;
        }

        public void OnEnd(SadWrapperComponent comp)
        {
            comp.AnimatePosition(comp.X, comp.Y);
        }

        public void OnStart(GameTime time, SadWrapperComponent comp){}

        public void Update(GameTime time, SadWrapperComponent comp)
        {
            _currentFrame++;
            if(_currentFrame > _framesPerHop)
            {
                if(_hopping)
                {
                    _hopping = false;
                    comp.AnimatePosition(comp.DrawX, comp.DrawY - _magnitude);
                } else
                {
                    _hopping = true;
                    comp.AnimatePosition(comp.DrawX, comp.DrawY + _magnitude);
                    _numHops++;
                }
                
                _currentFrame = 0;
                
            }
        }
    }
}
