using CsEcs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsEcs.SimpleEdits
{
    public class DoubleEdit : IIndexable
    {
        public double? NewValue { get; private set; }

        public DoubleEdit(double? value)
        {
            NewValue = value;
        }

        public string IndexKey => NewValue.ToString();
    }

}
