using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FSM_WPF_MinesweeperFun
{
    /// <summary>
    /// Interaction logic for ColorPickerPanel.xaml
    /// </summary>
    public partial class ColorPickerPanel : UserControl
    {
        public ColorPickerPanel()
        {
            InitializeComponent();
        }

        private void QuickColor_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                var colors = tag.Split(',');
                if (colors.Length == 3)
                {
                    if (byte.TryParse(colors[0], out byte r) &&
                        byte.TryParse(colors[1], out byte g) &&
                        byte.TryParse(colors[2], out byte b))
                    {
                        // Set the color values to the text boxes
                        RTextBox.Text = r.ToString();
                        GTextBox.Text = g.ToString();
                        BTextBox.Text = b.ToString();
                    }
                }
            }
        }
    }
}