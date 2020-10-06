namespace CsEcs.SimpleEdits
{
    public class PosEdit: IIndexable
    {
        public int? X { get; set; }
        public int? Y { get; set; }
        public PosEdit(int? x, int? y)
        {
            X = x;
            Y = y;
        }

        public string IndexKey => $"{X}/{Y}";
    }
}
