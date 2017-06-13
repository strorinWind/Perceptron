using AForge.Neuro;
using AForge.Neuro.Learning;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private Color[] colors = { Colors.Yellow, Colors.Blue, Colors.Red };
        private double[][] inputs;
        private double[][] outputs;
        private int hidden;
        private List<int> errors = new List<int>();
        private double[][] comput;
        private CancellationToken token;

        public MainWindow()
        {
            InitializeComponent();
            FillComboBox();
            ReadPoints("../../SquareCartesian.points");
            //ReadPoints("../../N.points");
        }

        private void FillComboBox()
        {
            for (int i = 1; i <= 15; i++)
            {
                var c = new ComboBoxItem();
                c.Content = "2:" + i + ":2";
                Combobox.Items.Add(c);
            }
            Combobox.Items.Add(new ComboBoxItem() { Content = "2:50:2" });
            Combobox.Items.Add(new ComboBoxItem() { Content = "2:100:2" });
            Combobox.SelectedIndex = 9;
        }

        private void ReadPoints(string path)
        {
            var m = File.ReadAllLines(path);
            inputs = new double[m.Length - 6][];
            outputs = new double[m.Length - 6][];
            for (int i = 6; i < m.Length; i++)
            {
                var a = m[i].Replace('.', ',').Split();
                var x = double.Parse(a[0]);
                var y = double.Parse(a[1]);
                inputs[i - 6] = new double[] { x, y };
                var c = double.Parse(a[2]);
                if (c == 1)
                {
                    outputs[i - 6] = new double[] { 1, 0 };
                }
                else
                {
                    outputs[i - 6] = new double[] { 0, 1 };
                }
            }
            DrawPoints(inputs, outputs);
        }

        private void DrawPoints(double[][] input,double[][] output)
        {
            var cns = PointField;
            cns.Children.Clear();
            cns.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            cns.Arrange(new Rect(0, 0, this.Width, this.Height));
            var width = cns.ActualWidth;
            var height = cns.ActualHeight;

            for (int i = 0; i < input.Length; i++)
            {
                var x = (input[i][0] + 1) * width / 3;
                var y = (input[i][1] + 1) * height / 2.7;
                double c = output[i][0] > output[i][1] ? 1 : 0;

                var rect = new Rectangle();
                rect.Width = 3;
                rect.Height = 3;
                //rect.Stroke = new SolidColorBrush(colors[c]);
                rect.Fill = new SolidColorBrush(colors[(int)Math.Round(c)]);
                Canvas.SetLeft(rect, x - 2);
                Canvas.SetTop(rect, y - 2);
                cns.Children.Add(rect);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawPoints(inputs,outputs);
        }

        private async void LearnBtn_Click(object sender, RoutedEventArgs e)
        {      
            var t = Task.Run(() => Learn());
            
            while (token.IsCancellationRequested!=true && t.IsCompleted != true)
            {                
                await Task.Delay(100);
                //Error.Text = (errors.Last() / inputs.Length * 100).ToString() + "%";
                Drawgraph(graph);
                //DrawPoints(inputs,comput);
                ComputePoints();
                
            }
            if (t.IsCompleted) MessageBox.Show("Learning finished");
            //t.Dispose();
        }

        private void Learn()
        {
            errors.Clear();
            var network = new ActivationNetwork(
            new SigmoidFunction(), // sigmoid activation function
            2,
            hidden,
            2);
            BackPropagationLearning teacher = new BackPropagationLearning(network);

            double error = 1;
            int k = inputs.Length;    
            while (k >= inputs.Length / 100)
            {
                error = teacher.RunEpoch(inputs, outputs);
                k = 0;
                var lst = new double[inputs.Length][];
                for (int i = 0; i < inputs.Length; i++)
                {
                    var c = network.Compute(inputs[i]);
                    if (c[0] > c[1] & outputs[i][0] < outputs[i][1] || c[0] < c[1] & outputs[i][0] > outputs[i][1])
                    {
                        k++;
                    }
                    lst[i] = c;
                }
                comput = lst;
                errors.Add(k);
            }
        }

        private void Combobox_Selected(object sender, RoutedEventArgs e)
        {
            var s = ((ComboBoxItem)Combobox.SelectedItem).Content.ToString().Split(':')[1];
            hidden = int.Parse(s);
        }

        private void Drawgraph(Canvas cns)
        {
            //var error = errors.Take(100).ToList();
            //graph.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //graph.Arrange(new Rect(0, 0, this.Width, this.Height));
            cns.Height = 50;
            cns.Children.Clear();
            if (errors.Count() > 0)
            {
                int m = 0;
                try { m = errors.Max(); }
                catch { m = errors[0]; }

                for (int i = 1; i < errors.Count; i++)
                {
                    var l = new Line();
                    l.X1 = (i - 1) * 1;
                    l.X2 = i * 1;
                    l.Y1 = 50 - (50 * errors[i - 1] / m);
                    l.Y2 = 50 - (50 * errors[i] / m);
                    l.Stroke = Brushes.Red;
                    l.StrokeThickness = 2;
                    cns.Children.Add(l);
                }
            }
        }

        private void ComputePoints()
        {
            var cns = PointField;
            cns.Children.Clear();
            cns.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            cns.Arrange(new Rect(0, 0, this.Width, this.Height));
            var width = cns.ActualWidth;
            var height = cns.ActualHeight;

            for (int i = 0; i < inputs.Length; i++)
            {
                var x = (inputs[i][0] + 1) * width / 3;
                var y = (inputs[i][1] + 1) * height / 2.7;
                double c = outputs[i][0] > outputs[i][1] ? 1 : 0;
                double s = comput[i][0] > comput[i][1] ? 1 : 0;

                var rect = new Rectangle();
                rect.Width = 3;
                rect.Height = 3;
                rect.Stroke = new SolidColorBrush(colors[(int)Math.Round(s)]);
                rect.StrokeThickness = 10;
                rect.Fill = new SolidColorBrush(colors[(int)Math.Round(c)]);
                Canvas.SetLeft(rect, x - 2);
                Canvas.SetTop(rect, y - 2);
                cns.Children.Add(rect);
            }
        }

        private void Open_Points(object sender, RoutedEventArgs e)
        {
            var c = new OpenFileDialog();
            c.DefaultExt = ".points";
            c.Filter = "Points |*.points";
            c.ShowDialog();
            if (c.FileName!="") ReadPoints(c.FileName);
        }

        /*
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            var cs = new CancellationTokenSource();
            token = cs.Token;
            cs.Cancel();
        }*/
    }
}
