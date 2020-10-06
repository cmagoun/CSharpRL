namespace CsEcs.SimpleEdits
{
    public class StringEdit:IIndexable
    {
        public string NewValue { get; set; }
        public StringEdit(string value)
        {
            NewValue = value;
        }

        public string IndexKey => NewValue;
    }
}
