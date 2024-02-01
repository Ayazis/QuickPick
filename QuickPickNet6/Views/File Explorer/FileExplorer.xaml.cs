using System.Windows;

namespace QuickPick.UI.Views.File_Explorer
{
    /// <summary>
    /// Interaction logic for FileExplorer.xaml
    /// </summary>
    public partial class FileExplorer : Window
    {
        File_Explorer_DataContext _context = new File_Explorer_DataContext();
        public FileExplorer(File_Explorer_DataContext context)
        {
            InitializeComponent(); // Initialise first, it will create a context. 
            _context = context;  // Override the default context.
            this.DataContext = _context;
        }
    }
}
