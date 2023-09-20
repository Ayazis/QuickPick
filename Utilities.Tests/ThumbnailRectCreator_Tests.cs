using NUnit.Framework;
using QuickPick.UI;
using System.Windows; // Make sure to add PresentationCore as a reference for the Point class
// Your namespace where ThumbnailRectCreator resides
namespace YourNamespace.Tests
{
    [TestFixture]
    public class ThumbnailRectCreatorTests
    {


        [Test]
        public void TestCreateRectForThumbnail_Landscape_LeftToCenter()
        {
            var creator = new ThumbnailRectCreator();
            var rect = creator.CreateRectForThumbnail(new Point(100, 100), -50, -50, 1, 0, 1.5);

            Assert.AreEqual(-130, rect.Left);
            Assert.AreEqual(-70, rect.Top);
            Assert.AreEqual(50, rect.Right); // This value should be correct now
            Assert.AreEqual(50, rect.Bottom);
        }
        [Test]
        public void TestCreateRectForThumbnail_Landscape_RightToCenter()
        {
            var creator = new ThumbnailRectCreator();
            var rect = creator.CreateRectForThumbnail(new Point(100, 100), 50, -50, 1, 0, 1.5);

            Assert.AreEqual(150, rect.Left);
            Assert.AreEqual(-70, rect.Top);
            Assert.AreEqual(330, rect.Right);
            Assert.AreEqual(50, rect.Bottom);
        }
        [Test]
        public void TestCreateRectForThumbnail_Landscape_RightToCenter_IncrementedIndex()
        {
            var creator = new ThumbnailRectCreator();
            var rect = creator.CreateRectForThumbnail(new Point(100, 100), 50, -50, 1, 1, 1.5);

            Assert.AreEqual(350, rect.Left);  // 150 (original Left) + 200 (i * 200)
            Assert.AreEqual(-70, rect.Top);
            Assert.AreEqual(530, rect.Right);  // 330 (original Right) + 200 (i * 200)
            Assert.AreEqual(50, rect.Bottom);
        }
        [Test]
        public void TestCreateRectForThumbnail_Landscape_BelowCenter()
        {
            var creator = new ThumbnailRectCreator();
            var rect = creator.CreateRectForThumbnail(new Point(100, 100), 50, 50, 1, 0, 1.5);

            Assert.AreEqual(150, rect.Left);
            Assert.AreEqual(150, rect.Top);  // Corrected Top value
            Assert.AreEqual(330, rect.Right);
            Assert.AreEqual(270, rect.Bottom); // Corrected Bottom value
        }
        [Test]
        public void TestCreateRectForThumbnail_Landscape_AboveCenter()
        {
            var creator = new ThumbnailRectCreator();
            var rect = creator.CreateRectForThumbnail(new Point(100, 100), 50, -50, 1, 0, 1.5);

            Assert.AreEqual(150, rect.Left);
            Assert.AreEqual(-70, rect.Top);  // 50 (original Top) - 120 (height)
            Assert.AreEqual(330, rect.Right);
            Assert.AreEqual(50, rect.Bottom); // 170 (original Bottom) - 120 (height)
        }


    }
}
