using Microsoft.Xna.Framework;
using NumberCruncher.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberCruncher.Animation
{
    public class CompoundAnimation : IAnimation
    {
        private List<IAnimation> _animations;
        private List<int> _complete;
        private List<int> _start;
        private List<int> _end;

        public bool IsRunning { get; set; }

        public CompoundAnimation(List<IAnimation> animations, List<int>complete = null,  List<int> start = null, List<int> end = null)
        {
            _animations = animations;
            _complete = complete;
            _start = start;
            _end = end;
        }
        public bool IsComplete(SadWrapperComponent comp)
        {
            var test =  _complete.All(index => _animations[index].IsComplete(comp));
            return test;
        }

        public void OnEnd(SadWrapperComponent comp)
        {
            if (_end == null) return;
            _end.ForEach(index => _animations[index].OnEnd(comp));
        }

        public void OnStart(GameTime time, SadWrapperComponent comp)
        {
            if (_start == null) return;
            _start.ForEach(index => _animations[index].OnStart(time, comp));
        }

        public void Update(GameTime time, SadWrapperComponent comp)
        {
            foreach(var animation in _animations)
            {
                animation.Update(time, comp);
            }
        }
    }
}
