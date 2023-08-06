using QuickPick.PinnedApps;
using System.Diagnostics;

namespace Utilities.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            List<AppLink> openWindows = AppLinkRetriever.GetPinnedAppsAndActiveWindows();
            Console.WriteLine();
            Console.WriteLine(openWindows.Count + " open windows on current Desktop");
            foreach (var item in openWindows)
            {
                Console.WriteLine(item.Name);
            }

        }
    }
}