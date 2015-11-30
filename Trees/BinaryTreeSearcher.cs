using System;
using System.Collections.Generic;

namespace Tree
{
    public interface BinaryTreeSearcher
    {
        IEnumerable<Int32> FindLevelsOf(Int32 value, Node tree);
    }
}
