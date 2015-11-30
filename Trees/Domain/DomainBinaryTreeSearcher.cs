using System;
using System.Collections.Generic;

namespace Tree.Domain
{
    public class DomainBinaryTreeSearcher : BinaryTreeSearcher
    {
        public IEnumerable<Int32> FindLevelsOf(Int32 value, Node tree)
        {
            var levels = new HashSet<Int32>();

            levels = SearchTreeFor(value, tree, levels, 1);

            return levels;
        }

        private HashSet<Int32> SearchTreeFor(Int32 value, Node node, HashSet<Int32> levels, Int32 level)
        {
            if (node == null)
                return levels;

            if (node.Value == value)
                levels.Add(level);

            levels = SearchTreeFor(value, node.Left, levels, level + 1);
            levels = SearchTreeFor(value, node.Right, levels, level + 1);

            return levels;
        }
    }
}
