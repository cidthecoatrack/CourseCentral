using NUnit.Framework;
using System;
using Tree;
using Tree.Domain;

namespace CourseCentral.Tests.Tree
{
    [TestFixture]
    public class DomainTreeIndexRepositoryTests
    {
        private TreeIndexRepository repository;

        [SetUp]
        public void Setup()
        {
            repository = new DomainTreeIndexRepository();
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(2, 0)]
        [TestCase(3, 1)]
        [TestCase(4, 1)]
        [TestCase(5, 2)]
        [TestCase(6, 2)]
        [TestCase(7, 3)]
        [TestCase(8, 3)]
        [TestCase(9, 4)]
        [TestCase(10, 4)]
        [TestCase(11, 5)]
        [TestCase(12, 5)]
        [TestCase(13, 6)]
        [TestCase(14, 6)]
        [TestCase(15, 7)]
        [TestCase(16, 7)]
        [TestCase(17, 8)]
        [TestCase(18, 8)]
        [TestCase(19, 9)]
        [TestCase(20, 9)]
        [TestCase(21, 10)]
        [TestCase(22, 10)]
        [TestCase(23, 11)]
        [TestCase(24, 11)]
        [TestCase(25, 12)]
        [TestCase(26, 12)]
        [TestCase(27, 13)]
        [TestCase(28, 13)]
        [TestCase(29, 14)]
        [TestCase(30, 14)]
        public void ParentNodeIsCorrect(Int32 nodeIndex, Int32 parentNodeIndex)
        {
            var parent = repository.GetParentIndex(nodeIndex);
            Assert.That(parent, Is.EqualTo(parentNodeIndex));
        }

        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        [TestCase(5, false)]
        [TestCase(6, false)]
        [TestCase(7, true)]
        [TestCase(8, true)]
        [TestCase(9, true)]
        [TestCase(10, true)]
        [TestCase(11, false)]
        [TestCase(12, false)]
        [TestCase(13, false)]
        [TestCase(14, false)]
        [TestCase(15, true)]
        [TestCase(16, true)]
        [TestCase(17, true)]
        [TestCase(18, true)]
        [TestCase(19, true)]
        [TestCase(20, true)]
        [TestCase(21, true)]
        [TestCase(22, true)]
        [TestCase(23, false)]
        [TestCase(24, false)]
        [TestCase(25, false)]
        [TestCase(26, false)]
        [TestCase(27, false)]
        [TestCase(28, false)]
        [TestCase(29, false)]
        [TestCase(30, false)]
        public void IsLeftIsCorrect(Int32 nodeIndex, Boolean isActuallyLeft)
        {
            var isLeft = repository.IsLeft(nodeIndex);
            Assert.That(isLeft, Is.EqualTo(isActuallyLeft));
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(2, 0)]
        [TestCase(3, 1)]
        [TestCase(4, 2)]
        [TestCase(5, 1)]
        [TestCase(6, 2)]
        [TestCase(7, 3)]
        [TestCase(8, 4)]
        [TestCase(9, 5)]
        [TestCase(10, 6)]
        [TestCase(11, 3)]
        [TestCase(12, 4)]
        [TestCase(13, 5)]
        [TestCase(14, 6)]
        [TestCase(15, 7)]
        [TestCase(16, 8)]
        [TestCase(17, 9)]
        [TestCase(18, 10)]
        [TestCase(19, 11)]
        [TestCase(20, 12)]
        [TestCase(21, 13)]
        [TestCase(22, 14)]
        [TestCase(23, 7)]
        [TestCase(24, 8)]
        [TestCase(25, 9)]
        [TestCase(26, 10)]
        [TestCase(27, 11)]
        [TestCase(28, 12)]
        [TestCase(29, 13)]
        [TestCase(30, 14)]
        public void TranposedIndexIsCorrect(Int32 nodeIndex, Int32 transposedIndex)
        {
            var transposition = repository.GetTransposedIndex(nodeIndex);
            Assert.That(transposition, Is.EqualTo(transposedIndex));
        }
    }
}
