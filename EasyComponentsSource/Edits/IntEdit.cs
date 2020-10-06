namespace CsEcs.SimpleEdits
{
    public class IntEdit:IIndexable
    {
        public int? NewValue { get; private set; }

        public IntEdit(int? value)
        {
            NewValue = value;
        }

        public string IndexKey => NewValue.ToString();
    }
}