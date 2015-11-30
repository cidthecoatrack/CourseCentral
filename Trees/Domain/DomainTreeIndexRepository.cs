using System;

namespace Tree.Domain
{
    public class DomainTreeIndexRepository : TreeIndexRepository
    {
        public Int32 GetParentIndex(Int32 nodeIndex)
        {
            if (nodeIndex == 0)
                return 0;

            var oddRemainder = nodeIndex % 2;
            var evenRemainder = (nodeIndex + 1) % 2 * 2;

            return (nodeIndex - oddRemainder - evenRemainder) / 2;
        }

        public Int32 GetTransposedIndex(Int32 nodeIndex)
        {
            var level = GetLevel(nodeIndex);

            if (level == 1)
                return 0;

            if (IsLeft(nodeIndex))
                return nodeIndex - Convert.ToInt32(Math.Pow(2, level - 2));

            return nodeIndex - Convert.ToInt32(Math.Pow(2, level - 1));
        }

        private Int32 GetLevel(Int32 nodeIndex)
        {
            var level = 1;
            var tempIndex = 0;

            while (tempIndex < nodeIndex)
            {
                tempIndex += Convert.ToInt32(Math.Pow(2, level));
                level++;
            }

            return level;
        }

        public Boolean IsLeft(Int32 nodeIndex)
        {
            var rootIndex = nodeIndex;

            while (rootIndex > 2)
                rootIndex = GetParentIndex(rootIndex);

            return rootIndex == 1;
        }
    }
}
