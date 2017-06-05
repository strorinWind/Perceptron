using System;
using System.Collections.Generic;
using System.IO;
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
        private Color[] colors = { Colors.Yellow, Colors.Blue, Colors.Red};

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawPoints()
        {
            var cns = PointField;
            cns.Children.Clear();
            cns.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            cns.Arrange(new Rect(0, 0, this.Width, this.Height));
            var width = cns.ActualWidth;
            var height = cns.ActualHeight;

            var m = File.ReadAllLines("../../N.points");
            for (int i = 6; i < m.Length; i++)
            {
                var a = m[i].Replace('.',',').Split();
                var x = (double.Parse(a[0])+1)*width/3;
                var y = (double.Parse(a[1])+1)*height/3;
                var c = int.Parse(a[2])-1;

                var rect = new Rectangle();
                rect.Width = 2;
                rect.Height = 2;
                //rect.Stroke = new SolidColorBrush(colors[c]);
                rect.Fill = new SolidColorBrush(colors[c]);
                Canvas.SetLeft(rect, x-2);
                Canvas.SetTop(rect, y-2);
                cns.Children.Add(rect);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {       
            DrawPoints();
        }
    }
}
