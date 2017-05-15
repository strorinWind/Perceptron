using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Perceptron.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DrawPoints();
        }

        private void DrawPoints()
        {
            var cns = PointField;
            for (int i = 0; i < 10; i++)
            {
                var rect = new Rectangle();
                rect.Width = 5;
                rect.Height = 5;
                rect.Stroke = new SolidColorBrush(Colors.White);
                rect.Fill = new SolidColorBrush(Colors.White);
                Canvas.SetLeft(rect, 50+i*20);
                Canvas.SetTop(rect, 70);
                cns.Children.Add(rect);

            }
        }
    }
}
