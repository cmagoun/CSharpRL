using CsEcs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyComponents
{
    public interface IMergeable
    {
        IComponent Merge(IComponent newComponent);
    }
}
