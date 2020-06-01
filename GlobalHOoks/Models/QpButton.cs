using GlobalHOoks.Enums;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GlobalHOoks.Classes
{
    public class QpButton : INotifyPropertyChanged
    {
        private string _AssociatedFilePath = "-";
        [JsonIgnore]
        public string AssociatedFilePath
        {
            get { return _AssociatedFilePath; }
            set
            {
                _AssociatedFilePath = value;
                var fileName = Path.GetFileNameWithoutExtension(value);
                var fullName = Path.GetFileName(value);
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    this.FileName = fullName;
                    Icon.ToolTip = fileName;
                    Button.ToolTip = fileName;
                }
                NotifyPropertyChanged(nameof(AssociatedFilePath));
            }
        }

        public string FileName { get; set; }

        [JsonIgnore]
        public ActionDelegate Act { get; set; }

        public delegate void ActionDelegate(QpButton button);


        private ObservableCollection<ClickAction> _ClickActions = new ObservableCollection<ClickAction> {
             ClickAction.None
            ,ClickAction.RunProcess
            ,ClickAction.RunQuery
            ,ClickAction.TakeSnippet
            ,ClickAction.ExitQuickPick
        };
        [JsonIgnore]
        public ObservableCollection<ClickAction> ClickActions
        {
            get { return _ClickActions; }
            set
            {
                _ClickActions = value;
                NotifyPropertyChanged(nameof(ClickActions));
            }
        }

        private ClickAction _ActionType = ClickAction.None;
        public ClickAction ActionType
        {
            get { return _ActionType; }
            set
            {
                _ActionType = value;
                NotifyPropertyChanged(nameof(ActionType));
            }
        }

        public string IconLocation { get; set; }
        [JsonIgnore]
        public Image Icon { get; set; }

        [JsonIgnore]
        public Button Button { get; set; }
        [JsonIgnore]
        public Thickness Margin { get; set; }

        private int Width { get; set; } = 35;
        private int Height { get; set; } = 35;
        public int Id { get; set; }

        public QpButton()
        {
            Button = new Button
            {
                Width = this.Width
                ,
                Height = this.Height
            };
            Button.Click += Button_Click;
            Button.MouseEnter += Button_MouseEnter;
            Button.MouseLeave += Button_MouseLeave;

            Icon = new Image();
            Icon.MouseDown += Button_Click;
            Icon.MouseEnter += Icon_MouseEnter;
            Icon.MouseLeave += Icon_MouseLeave;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Act != null)
                Act(this);
        }
        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Button.Width = this.Button.Height -= 5;
        }
        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Button.Width = this.Button.Height += 5;
        }
        private void Icon_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Icon.Width = this.Icon.Height += 5;
        }
        private void Icon_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Icon.Width = this.Icon.Height -= 5;
        }
    }
}
