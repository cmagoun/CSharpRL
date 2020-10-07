using Microsoft.Xna.Framework;
using NumberCruncher.Components;

namespace ReferenceGame.Modes.Entity
{
    public class SlideAnimaion : IAnimation
    {
        public bool IsRunning { get; set; }
        private string _entityId;
        private Point _from;
        private Point _to;
        private float _speed;
        private Vector2 _current;
        private Vector2 _end;
        private Vector2 _direction;

        public SlideAnimaion(string entityId, Point from, Point to, float speed)
        {
            _speed = speed;
            _entityId = entityId;
            _from = from;
            _to = to;
            _current = new Vector2(from.X, from.Y);
            _end = new Vector2(to.X, to.Y);
            
            var sub = _to - _from;
            var vec = new Vector2(sub.X, sub.Y);
            vec.Normalize();

            _direction = vec;
        }

        public bool IsComplete(SadWrapperComponent comp)
        {
            var distance = _current - _end;
            return distance.LengthSquared() < 0.05f;
        }

        public void OnEnd(SadWrapperComponent comp)
        {
            comp.DoEdit(SadWrapperEdit.ChangePosition(_to.X, _to.Y));
        }

        public void OnStart(GameTime time, SadWrapperComponent comp)
        {
            var edit = SadWrapperEdit.AnimatePosition(_from.X, _from.Y);
            comp.DoEdit(edit);
        }

        public void Update(GameTime time, SadWrapperComponent comp)
        {
            var velocity = new Vector2(_direction.X * _speed, _direction.Y * _speed);
            _current = _current + velocity;

            comp.DoEdit(SadWrapperEdit.AnimatePosition(_current.X, _current.Y));
        }
    }
}
