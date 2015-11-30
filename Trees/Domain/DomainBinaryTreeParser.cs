using System;

namespace Tree.Domain
{
    public class DomainBinaryTreeParser : BinaryTreeParser
    {
        private TreeIndexRepository treeIndexRepository;

        public DomainBinaryTreeParser(TreeIndexRepository treeIndexRepository)
        {
            this.treeIndexRepository = treeIndexRepository;
        }

        public Node Parse(String input)
        {
            if (String.IsNullOrEmpty(input))
                return null;

            var sections = input.Split(',');
            var tree = new Node();

            for (var i = 0; i < sections.Length; i++)
                if (String.IsNullOrEmpty(sections[i]) == false)
                    Add(tree, Convert.ToInt32(sections[i]), i);

            return tree;
        }

        private void Add(Node node, Int32 value, Int32 nodeIndex)
        {
            if (nodeIndex <= 0)
            {
                node.Value = value;
                return;
            }

            var isLeft = treeIndexRepository.IsLeft(nodeIndex);
            var newNodeIndex = treeIndexRepository.GetTransposedIndex(nodeIndex);

            if (isLeft)
            {
                if (node.Left == null)
                    node.Left = new Node();

                Add(node.Left, value, newNodeIndex);
            }
            else
            {
                if (node.Right == null)
                    node.Right = new Node();

                Add(node.Right, value, newNodeIndex);
            }
        }

        private Int32 GetValue(String section)
        {
            if (String.IsNullOrEmpty(section))
                return 0;

            return Convert.ToInt32(section);
        }
    }
}
