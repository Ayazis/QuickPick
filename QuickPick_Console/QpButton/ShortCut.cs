using System.Drawing;

namespace QuickPick.Models
{
    public struct ShortCut
    {
        public string TargetPath { get; set; }
        public string IconLocation { get; set; }
        public Icon Icon { get; set; }
    }
}
