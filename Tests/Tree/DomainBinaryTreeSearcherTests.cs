using Ninject;
using NUnit.Framework;
using System.Linq;
using Tree;

namespace CourseCentral.Tests.Tree
{
    [TestFixture]
    public class DomainBinaryTreeSearcherTests : IntegrationTests
    {
        [Inject]
        public BinaryTreeSearcher BinaryTreeSearcher { get; set; }
        [Inject]
        public BinaryTreeParser BinaryTreeParser { get; set; }

        [Test]
        public void FindNoLevelsInEmptyTree()
        {
            var levels = BinaryTreeSearcher.FindLevelsOf(9266, null);
            Assert.That(levels, Is.Empty);
        }

        [Test]
        public void FindNoLevelsWhenNotInTree()
        {
            var tree = BinaryTreeParser.Parse("4,5,9,,6,3,5,,,,,,,2,7");
            var levels = BinaryTreeSearcher.FindLevelsOf(9266, tree);
            Assert.That(levels, Is.Empty);
        }

        [Test]
        public void FindOneLevelWhenInFirstRowOfTree()
        {
            var tree = BinaryTreeParser.Parse("4,5,9,,6,3,5,,,,,,,2,7");
            var levels = BinaryTreeSearcher.FindLevelsOf(4, tree);
            Assert.That(levels, Contains.Item(1));
            Assert.That(levels.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FindOneLevelWhenInTree()
        {
            var tree = BinaryTreeParser.Parse("4,5,9,,6,3,5,,,,,,,2,7");
            var levels = BinaryTreeSearcher.FindLevelsOf(6, tree);
            Assert.That(levels, Contains.Item(3));
            Assert.That(levels.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FindMultipleLevelsWhenInTree()
        {
            var tree = BinaryTreeParser.Parse("4,5,9,,6,3,5,,,,,,,2,7");
            var levels = BinaryTreeSearcher.FindLevelsOf(5, tree);
            Assert.That(levels, Contains.Item(2));
            Assert.That(levels, Contains.Item(3));
            Assert.That(levels.Count(), Is.EqualTo(2));
        }

        [Test]
        public void FindOneLevelForMultipleOccurances()
        {
            var tree = BinaryTreeParser.Parse("4,5,5,,6,3,9,,,,,,,2,7");
            var levels = BinaryTreeSearcher.FindLevelsOf(5, tree);
            Assert.That(levels, Contains.Item(2));
            Assert.That(levels.Count(), Is.EqualTo(1));
        }
    }
}
