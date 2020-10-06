using RogueSharp;

namespace SadSharp.MapCreators
{
    public class RogueCell:ICell
    {
        public bool Equals(ICell other)
        {
            if (other == null) return false;
            return X == other.X && Y == other.Y;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public bool IsTransparent { get; set; }
        public bool IsWalkable { get; set; }

        //records if this space is NOT transparent or walkable due to a mobile thing
        public bool IsMobile { get; set; }

        public bool IsRoom { get; set; }

        public void ClearMobileLosBlocker()
        {
            IsTransparent = true;
            IsMobile = false;
        }

        public void SetMobileLosBlocker()
        {
            IsTransparent = false;
            IsMobile = true;
        }


        public RogueCell SetTransparent(bool newValue)
        {
            IsTransparent = newValue;
            return this;
        }

        public RogueCell SetWalkable(bool newValue)
        {
            IsWalkable = newValue;
            return this;
        }

        public RogueCell SetMobile(bool newValue)
        {
            IsMobile = newValue;
            return this;
        }
    }
}
