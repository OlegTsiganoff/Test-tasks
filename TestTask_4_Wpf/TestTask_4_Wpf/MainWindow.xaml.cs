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

namespace TestTask_4_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window
    {
        List<Point> listResult;
        List<Point> listSource;
        int topPointIndex;
        int bottomPointIndex;
                
        public MainWindow()
        {
            InitializeComponent();
            MyInit();
        }

        void MyInit()
        {
            buttonGo.Click += buttonGo_Click;
            buttonReset.Click += buttonReset_Click;
            checkBox.Checked += checkBox_Checked;
            checkBox.Unchecked += checkBox_Unchecked;
            canvas.MouseLeftButtonDown += canvas_MouseLeftButtonDown;
            panelCoordinates.Visibility = System.Windows.Visibility.Hidden;
            txtDescription.Visibility = System.Windows.Visibility.Visible;
            KeyDown += MainWindow_KeyDown;
            buttonSetPoint.Click += buttonSetPoint_Click;
            listResult = new List<Point>();
            listSource = new List<Point>();
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetButton();
                textBox_X.Focus();
            }
        }


        
        void buttonSetPoint_Click(object sender, RoutedEventArgs e)
        {
            SetButton();            
        }

        void SetButton()
        {
            string xStr = textBox_X.Text;
            string yStr = textBox_Y.Text;

            int x, y;
            if (Int32.TryParse(xStr, out x) && Int32.TryParse(yStr, out y))
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Width = 3;
                ellipse.Height = 3;
                ellipse.Fill = new SolidColorBrush(Colors.Black);
                canvas.Children.Add(ellipse);
                Canvas.SetTop(ellipse, y);
                Canvas.SetLeft(ellipse, x);
                textBox_X.Text = "";
                textBox_Y.Text = "";
            }
            else
            {
                MessageBox.Show("Values mast be positive integer and less then 710");
            }
        }

        void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            txtDescription.Visibility = System.Windows.Visibility.Visible;
            panelCoordinates.Visibility = System.Windows.Visibility.Hidden;
        }

        void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            txtDescription.Visibility = System.Windows.Visibility.Hidden;
            panelCoordinates.Visibility = System.Windows.Visibility.Visible;
            textBox_X.Focus();
        }

        void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            listSource.Clear();
            listResult.Clear();
            for(int i = 0; i < canvas.Children.Count;)
            {
                UIElement ui = canvas.Children[i];
                if (ui is Line || ui is Ellipse) // remove all dots and lines from canvas
                    canvas.Children.Remove(ui);
                else
                    i++;
            }
        }

        void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            FillpointList();

            for (int i = 0; i < listResult.Count - 1; i++)
            {
                Line line = new Line();
                line.X1 = listResult.ElementAt(i).X + 1.5; // the radius of the circle is 1.5 
                line.Y1 = listResult.ElementAt(i).Y + 1.5;
                line.X2 = listResult.ElementAt(i + 1).X + 1.5;
                line.Y2 = listResult.ElementAt(i + 1).Y + 1.5;
                line.StrokeThickness = 2;
                line.Stroke = System.Windows.Media.Brushes.Black;
                canvas.Children.Add(line);
            }
        }

        void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // add dot to canvas
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 3;
            ellipse.Height = 3;
            ellipse.Fill = new SolidColorBrush(Colors.Black);
            canvas.Children.Add(ellipse);            
            Canvas.SetTop(ellipse, e.GetPosition(canvas).Y);
            Canvas.SetLeft(ellipse, e.GetPosition(canvas).X);
        }

        // main function
        // sort and add dots to the result list 
        // and then we can connect dots broken line
        void FillpointList()
        {
            if(canvas != null && canvas.Children.Count > 2)
            {
                // fill source point list
                foreach(UIElement elem in canvas.Children)
                {
                    if(elem is Ellipse)
                    {
                        listSource.Add(new Point(Canvas.GetLeft(elem), Canvas.GetTop(elem)));
                    }
                }              
                
                // if number of points less then 2, fill them result list and exit
                if(listSource.Count <= 2)
                {
                    foreach (Point p in listSource)
                        listResult.Add(p);
                    return;
                }

                // find top and bottom point
                int index = -1;
                double yTop = 0;
                double yBottom = 0;

                List<int> topList = new List<int>();
                List<int> bottomList = new List<int>();
                foreach(Point p in listSource)
                {
                    if(index == -1)
                    {
                        yTop = p.Y;
                        yBottom = p.Y;
                        index = 1;
                        continue;
                    }

                    if (p.Y < yTop)
                    {
                        yTop = p.Y;                        
                    }

                    if(p.Y > yBottom)
                    {
                        yBottom = p.Y;
                    }
                }
                
                // find all top and bottom points (its can lie on a horizontal line)
                foreach(Point p in listSource)
                {
                    if(p.Y == yTop)
                    {
                        topList.Add(listSource.IndexOf(p));
                    }

                    if(p.Y == yBottom)
                    {
                        bottomList.Add(listSource.IndexOf(p));
                    }
                }

                // далее может быть несколько вариантов
                // 1 - есть одна верхняя (нижняя) точка
                // 2 - верхних(нижнмх) точек несколько (лежат на одной линии по у)
                // 3 - вехние и нижние точки лежат на одной линии, т. е. все точки лежат на одной горизонтальной линии
                                
                // if all dots lie on one line
                if(topList.Count == listSource.Count)
                {
                    foreach (Point p in listSource) 
                        listResult.Add(p);
                    return;
                }                

                topPointIndex = topList.ElementAt(0);
                if (topList.Count > 1) // if we have several top points we choose leftmost (with smallest value of x)                
                {
                    double xLeft = listSource.ElementAt(topPointIndex).X;

                    foreach (int i in topList)
                    {
                        if (xLeft > listSource.ElementAt(i).X)
                        {
                            xLeft = listSource.ElementAt(i).X;
                            topPointIndex = i;
                        }
                    }
                }

                bottomPointIndex = bottomList.ElementAt(0);
                if (bottomList.Count > 1) // if we have several bottom points we choose leftmost (with smallest value of x)
                {
                    double xLeft = listSource.ElementAt(bottomPointIndex).X;

                    foreach (int i in bottomList)
                    {
                        if (xLeft > listSource.ElementAt(i).X)
                        {
                            xLeft = listSource.ElementAt(i).X;
                            bottomPointIndex = i;
                        }
                    }
                }

                List<int> leftPointsIndex = new List<int>();
                List<int> rightPointsIndex = new List<int>();

                double x1 = listSource.ElementAt(topPointIndex).X;
                double x2 = listSource.ElementAt(bottomPointIndex).X;
                double y1 = listSource.ElementAt(topPointIndex).Y;
                double y2 = listSource.ElementAt(bottomPointIndex).Y;

                // straight formula y = kx + b;
                // b = (x2 * y1 - y2 * x1) / (x2 - x1)
                // k = (y2 - y1) / (x2 - x1)             
                double k = 0;
                bool isVertical = false;
                if(x2 != x1)
                    k = (y2 - y1) / (x2 - x1);
                else // if x2 = x1, the straight vertical
                {
                    isVertical = true;
                    foreach(Point p in listSource)
                    {
                        // as we choose leftmost top and bottom points
                        // therefore other points we put to right-side list
                        if (listSource.IndexOf(p) != topPointIndex && listSource.IndexOf(p) != bottomPointIndex)
                        {
                            rightPointsIndex.Add(listSource.IndexOf(p));
                        }
                    }
                }

                
                // if x2 = x1, the straight vertical
                // if k negative, the straight tilted to the right
                // if k positive, the straight tilted to the left                

                // b = (x2 * y1 - y2 * x1) / (x2 - x1)
                double b = 0;
                if(x2 != x1)
                    b = (x2 * y1 - y2 * x1) / (x2 - x1);
                
                
                // next split cloud of dots on two parts by virtual line, left and right
                // left part include all dots on the left from line
                // othe dots in the right part
                if(isVertical == false)
                    foreach (Point p in listSource)
                    {
                        if (listSource.IndexOf(p) != topPointIndex && listSource.IndexOf(p) != bottomPointIndex) // exclude top and bottom points
                        {
                            double yTmp = k * p.X + b; // coordinates of points on our line for a given x
                            if (k > 0)
                            {
                                if (p.Y < yTmp)
                                {
                                    leftPointsIndex.Add(listSource.IndexOf(p));
                                }
                                else
                                {
                                    rightPointsIndex.Add(listSource.IndexOf(p));
                                }
                            }
                            else if (k < 0)
                            {
                                if (p.Y > yTmp)
                                {
                                    leftPointsIndex.Add(listSource.IndexOf(p));
                                }
                                else
                                {
                                    rightPointsIndex.Add(listSource.IndexOf(p));
                                }
                            }

                        }
                    }
                
                // next sort points in the list of left-points ascending y
                // and sort list of right-points descending y
                List<Point> tmpList = new List<Point>();
                foreach(int i in leftPointsIndex)
                {
                    tmpList.Add(listSource.ElementAt(i));
                }

                var list1 = tmpList.OrderBy(p => p.Y).ToList();
                tmpList.Clear();
                foreach(int i in rightPointsIndex)
                {
                    tmpList.Add(listSource.ElementAt(i));
                }

                var list2 = tmpList.OrderByDescending(p => p.Y).ToList();
                
                // next fill the result list in order to connect dots broken line
                listResult.Add(listSource.ElementAt(topPointIndex)); // first top point
                foreach(Point p in list1) // next all dots from left-side list
                {                    
                    listResult.Add(p);
                }

                listResult.Add(listSource.ElementAt(bottomPointIndex)); // then bottom point

                foreach(Point p in list2) // then all dots on the right side
                {
                    listResult.Add(p);
                }

                listResult.Add(listSource.ElementAt(topPointIndex)); // and in the end top point
            }
        }
    }
}
