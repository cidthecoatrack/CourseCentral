using System;

namespace Tree
{
    public interface TreeIndexRepository
    {
        Int32 GetParentIndex(Int32 nodeIndex);
        Boolean IsLeft(Int32 nodeIndex);
        Int32 GetTransposedIndex(Int32 nodeIndex);
    }
}
