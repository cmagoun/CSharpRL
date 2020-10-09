using CsEcs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyComponents
{
    public interface IMergable
    {
        IComponent Merge(IComponent newComponent);
    }
}
