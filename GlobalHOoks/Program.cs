using GlobalHOoks.Logic;
using System;
using System.Windows.Forms;

namespace GlobalHOoks
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var manager = new WindowManager();
                Application.Run(new ApplicationContext());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }     
    }
}
