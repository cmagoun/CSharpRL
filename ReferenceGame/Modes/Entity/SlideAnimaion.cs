using Microsoft.Xna.Framework;
using ReferenceGame.Components;

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

        public bool IsComplete(EntityWrapperComponent comp)
        {
            var distance = _current - _end;
            return distance.LengthSquared() < 0.05f;
        }

        public void OnEnd(EntityWrapperComponent comp)
        {
            comp.DoEdit(EntityWrapperEdit.ChangePosition(_to.X, _to.Y));
        }

        public void OnStart(GameTime time, EntityWrapperComponent comp)
        {
            var edit = EntityWrapperEdit.AnimatePosition(_from.X, _from.Y);
            comp.DoEdit(edit);
        }

        public void Update(GameTime time, EntityWrapperComponent comp)
        {
            var velocity = new Vector2(_direction.X * _speed, _direction.Y * _speed);
            _current = _current + velocity;

            comp.DoEdit(EntityWrapperEdit.AnimatePosition(_current.X, _current.Y));
        }
    }
}
