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
            string path = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe";
            //WindowActivator.ActivateWindowOnCurrentVirtualDesktop(path);
            string hexValue = "0x000000000001046c";
            long longValue = Convert.ToInt64(hexValue, 16);
            IntPtr intptrValue = new IntPtr(longValue);

            // Maximise: works
            //WindowActivator.ActivateWindow(intptrValue,3);

            ActiveWindows.ToggleWindow(intptrValue);


            //Assert.Pass();
        }
    }
}