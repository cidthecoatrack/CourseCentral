using Ninject;
using NUnit.Framework;
using System;
using Tree;

namespace CourseCentral.Tests.Tree
{
    [TestFixture]
    public class DomainBinaryTreeParserTests : IntegrationTests
    {
        [Inject]
        public BinaryTreeParser BinaryTreeParser { get; set; }

        [TestCase("2,13,55", 2, 13, 55)]
        [TestCase("3,6,8", 3, 6, 8)]
        public void TwoLevelBinaryTree(String input, Int32 root, Int32 left, Int32 right)
        {
            var tree = BinaryTreeParser.Parse(input);
            Assert.That(tree, Is.Not.Null);
            Assert.That(tree.Value, Is.EqualTo(root));
            AssertLeafNode(tree.Left, left);
            AssertLeafNode(tree.Right, right);
        }

        public void AssertLeafNode(Node node, Int32 value)
        {
            Assert.That(node.Value, Is.EqualTo(value));
            Assert.That(node.Left, Is.Null);
            Assert.That(node.Right, Is.Null);
        }

        [Test]
        public void EmptyStringIsEmptyTree()
        {
            var tree = BinaryTreeParser.Parse(String.Empty);
            Assert.That(tree, Is.Null);
        }

        [Test]
        public void FourLevelBinaryTree()
        {
            var tree = BinaryTreeParser.Parse("0,1,2,3,4,5,6,7,8,9,10,11,12,13,14");
            Assert.That(tree, Is.Not.Null);
            Assert.That(tree.Value, Is.EqualTo(0));
            Assert.That(tree.Left.Value, Is.EqualTo(1));
            Assert.That(tree.Right.Value, Is.EqualTo(2));
            Assert.That(tree.Left.Left.Value, Is.EqualTo(3));
            Assert.That(tree.Left.Right.Value, Is.EqualTo(4));
            Assert.That(tree.Right.Left.Value, Is.EqualTo(5));
            Assert.That(tree.Right.Right.Value, Is.EqualTo(6));
            AssertLeafNode(tree.Left.Left.Left, 7);
            AssertLeafNode(tree.Left.Left.Right, 8);
            AssertLeafNode(tree.Left.Right.Left, 9);
            AssertLeafNode(tree.Left.Right.Right, 10);
            AssertLeafNode(tree.Right.Left.Left, 11);
            AssertLeafNode(tree.Right.Left.Right, 12);
            AssertLeafNode(tree.Right.Right.Left, 13);
            AssertLeafNode(tree.Right.Right.Right, 14);
        }

        [Test]
        public void FourLevelBinaryTreeWithMissingNodes()
        {
            var tree = BinaryTreeParser.Parse("4,5,9,,6,3,5,,,,,,,2,7");
            Assert.That(tree, Is.Not.Null);
            Assert.That(tree.Value, Is.EqualTo(4));
            Assert.That(tree.Left.Value, Is.EqualTo(5));
            Assert.That(tree.Right.Value, Is.EqualTo(9));
            Assert.That(tree.Left.Left, Is.Null);
            AssertLeafNode(tree.Left.Right, 6);
            AssertLeafNode(tree.Right.Left, 3);
            Assert.That(tree.Right.Right.Value, Is.EqualTo(5));
            AssertLeafNode(tree.Right.Right.Left, 2);
            AssertLeafNode(tree.Right.Right.Right, 7);
        }
    }
}
