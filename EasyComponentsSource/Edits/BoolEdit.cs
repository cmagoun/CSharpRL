namespace CsEcs.SimpleEdits
{
    public class BoolEdit
    {
        public bool? NewValue { get; private set; }

        public BoolEdit(bool? value)
        {
            NewValue = value;
        }
    }
}
