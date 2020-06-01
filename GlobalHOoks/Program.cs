using System;
using System.Windows.Forms;

namespace GlobalHOoks
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {          
            new WindowManager();
            Application.Run(new ApplicationContext());    
        }     
    }
}
