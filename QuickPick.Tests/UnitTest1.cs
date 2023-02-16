using NUnit.Framework;
using QuickPick.SRC.Logic;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace QuickPick.Tests
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
			var thumnails = ActiveApps.GetAllOpenWindows().ToList();
			
		}
	}
}