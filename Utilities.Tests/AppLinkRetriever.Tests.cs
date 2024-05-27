using QuickPick.PinnedApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Tests;
public class AppLinkRetrieverTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetAppLinks()
    {
        var apps = AppLinkRetriever.GetPinnedAppsAndActiveWindows(includePinnedApps: true);
    }
}
