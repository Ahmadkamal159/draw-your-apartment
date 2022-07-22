using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace draw_your_apartment_WPF_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        #region member_fields_to_help_in complie_time_and_runtime
        Point LineStartPos = new Point();
        static Point LineEndPos = new Point();
        static Point MovingPoint;
        Line line = new Line();
        Line Fakeline;
        static int count = 0;
        static int i;//to count fake/guide lines
        static int j;//to count vertical lines
        static int k;//to count horizontal lines
        static int L;//to count vertical doors
        static int M;//to count vertical windows
        static int N;//to count horizontal doors
        static int O;//to count horizontal windows
        static bool DeleteElement=false;//to check the if the delete mode is on or off
        #endregion

        #region handling_on_click_action

        private void ElibreCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            //the next two lines to check if mouse is clicked over line or image
            Line l = ElibreCanvas.Children.OfType<Line>().FirstOrDefault(x => x.IsMouseDirectlyOver);
            Image IMG= ElibreCanvas.Children.OfType<Image>().FirstOrDefault(x => x.IsMouseDirectlyOver);
            if (l == null)//to handle null exceptions
            {
                l = new Line() { X1=0,X2=0.001,Y1=0,Y2=0.001};
            }
            if(IMG== null)//to handle null exceptions
            {
                IMG=new Image() { Height=0,Width=0};   
            }
            #region checking_the_clicked_points_is_it_start_or_end_Line's_point

            if (e.LeftButton == MouseButtonState.Pressed && l.IsMouseOver==false&&IMG.IsMouseDirectlyOver==false&& DeleteElement==false)
            {
                switch (count)
                {
                    case 0:
                        LineStartPos = e.GetPosition(ElibreCanvas); //get the first point of the line when first click the mouse's left button
                        count++;
                        break;

                    case 1:
                        LineEndPos = e.GetPosition(ElibreCanvas);
                        count++;
                        break;
                    case 2:
                        count = 1;
                        LineStartPos = e.GetPosition(ElibreCanvas);
                        break;
                  
                }
              
            }
            #endregion

            #region lines_are_being_drawn_here

            if (count == 2 &&DeleteElement==false)
            {
                double ratio = (Math.Abs(LineStartPos.Y - LineEndPos.Y)) / (Math.Abs(LineStartPos.X - LineEndPos.X));

                if (ratio > 1)//draw vertical Lines
                {
                    ++j;
                    line = new Line() { Uid=$"vertical line no{j}", StrokeThickness=2, Stroke = SystemColors.ControlDarkDarkBrush, X1 = LineStartPos.X, Y1 = LineStartPos.Y, X2 = LineStartPos.X, Y2 = LineEndPos.Y };


                }
                else //to draw horizontal lines*/
                {
                    ++k;
                    line = new Line() {Uid= $"horizontal line no{k}", StrokeThickness=2, Stroke = SystemColors.ControlDarkDarkBrush, X1 = LineStartPos.X, Y1 = LineStartPos.Y, X2 = LineEndPos.X, Y2 = LineStartPos.Y };

                }
                ElibreCanvas.Children.Add(line);
                
                
                
                foreach (Line line in ElibreCanvas.Children.OfType<Line>().ToList()) //Delete the final guide line when clicking left button
                {
                    if (line.Uid.Contains("guideline"))
                    {

                        ElibreCanvas.Children.Remove(line);
                    }
                }
                ElibreCanvas.UpdateLayout();
            }
            #endregion

            #region doors_are_being_generated_here

            if (l!=null && l.IsMouseDirectlyOver==true && e.LeftButton == MouseButtonState.Pressed &&DeleteElement==false)
            {
                
                if(l.Uid.Contains("vertical line")&&IMG.IsMouseDirectlyOver==false)//draw vertical door
                {
                    ++L;
                    Image VDoor = new Image();
                    BitmapImage srcDoorH = new BitmapImage();
                    srcDoorH.BeginInit();
                    srcDoorH.UriSource = new Uri("pack://application:,,,/DoorsAndWindows/VerticalDoor.png", UriKind.Absolute);
                    srcDoorH.EndInit();
                    VDoor.Source = srcDoorH;
                    VDoor.Uid = $"VDoor{L}";
                    VDoor.Width = 55;
                    VDoor.Height = 55;
                    Canvas.SetTop(VDoor, e.GetPosition(ElibreCanvas).Y-(VDoor.Height/2));
                    Canvas.SetLeft(VDoor, e.GetPosition(ElibreCanvas).X);
                    ElibreCanvas.Children.Add(VDoor);
                    ElibreCanvas.UpdateLayout();


                }
                else if(l.Uid.Contains("horizontal line")&&IMG.IsMouseDirectlyOver==false)//draw horizontal door
                {
                    ++N;
                    Image HDoor = new Image();
                    BitmapImage srcDoorH = new BitmapImage();
                    srcDoorH.BeginInit();
                    srcDoorH.UriSource = new Uri("pack://application:,,,/DoorsAndWindows/HorizontalDoor.png", UriKind.Absolute);
                    srcDoorH.EndInit();
                    HDoor.Source = srcDoorH;
                    HDoor.Uid = $"HDoor{N}";
                    HDoor.Width = 55;
                    HDoor.Height = 55;
                    Canvas.SetTop(HDoor, e.GetPosition(ElibreCanvas).Y-HDoor.Height);
                    Canvas.SetLeft(HDoor, e.GetPosition(ElibreCanvas).X-(HDoor.Width/2));
                    ElibreCanvas.Children.Add(HDoor);
                    ElibreCanvas.UpdateLayout();
                }
            }
            #endregion

            #region windows_are_being_generated_here

            if (IMG != null && IMG.IsMouseDirectlyOver == true && e.LeftButton == MouseButtonState.Pressed&&DeleteElement==false)
            {
                if (IMG.Uid.Contains("VDoor"))//vertical window is created when vertical door is clicked
                {
                    ++M;
                    var deleteddoor=ElibreCanvas.Children.OfType<Image>().ToList().FirstOrDefault(x => x.IsMouseDirectlyOver);
                    
                    Image VWindow = new Image();
                    BitmapImage srcWindowV = new BitmapImage();
                    srcWindowV.BeginInit();
                    srcWindowV.UriSource = new Uri("pack://application:,,,/DoorsAndWindows/VerticalWindow.jpg", UriKind.Absolute);
                    srcWindowV.EndInit();
                    VWindow.Source = srcWindowV;
                    VWindow.Uid = $"Vwindow{M}";
                    VWindow.Width = 20;
                    VWindow.Height = 55;
                    Canvas.SetTop(VWindow, Canvas.GetTop(deleteddoor));
                    Canvas.SetLeft(VWindow, Canvas.GetLeft(deleteddoor)-10);
                    ElibreCanvas.Children.Remove(deleteddoor);
                    ElibreCanvas.Children.Add(VWindow);
                    ElibreCanvas.UpdateLayout();
                }
                else if (IMG.Uid.Contains("HDoor"))//vertical window is created when vertical door is clicked
                {
                    ++O;
                    var deleteddoor = ElibreCanvas.Children.OfType<Image>().ToList().FirstOrDefault(x => x.IsMouseDirectlyOver);

                    Image HWindow = new Image();
                    BitmapImage srcWindowH = new BitmapImage();
                    srcWindowH.BeginInit();
                    srcWindowH.UriSource = new Uri("pack://application:,,,/DoorsAndWindows/HorizontalWindow.jpg", UriKind.Absolute);
                    srcWindowH.EndInit();
                    HWindow.Source = srcWindowH;
                    HWindow.Uid = $"Hwindow{O}";
                    HWindow.Width = 55;
                    HWindow.Height = 20;

                    Canvas.SetTop(HWindow, Canvas.GetTop(deleteddoor)+43.75);
                    Canvas.SetLeft(HWindow, Canvas.GetLeft(deleteddoor));
                    ElibreCanvas.Children.Remove(deleteddoor);
                    ElibreCanvas.Children.Add(HWindow);
                    ElibreCanvas.UpdateLayout();
                }
            }
            #endregion

            #region deleting_clicked_elments_occurs_here

            if (DeleteElement == true && e.LeftButton == MouseButtonState.Pressed)
            {
                Line DLTLine = ElibreCanvas.Children.OfType<Line>().ToList().FirstOrDefault(x => x.IsMouseDirectlyOver);
                Image DLTImg = ElibreCanvas.Children.OfType<Image>().ToList().FirstOrDefault(x => x.IsMouseDirectlyOver);
                if(DLTLine == null)
                {
                    DLTLine = new Line() { X1 = 0, X2 = 0.001, Y1 = 0, Y2 = 0.001 };
                }
                if (DLTImg == null)
                {
                    DLTImg = new Image() { Height = 0, Width = 0 };
                }

                if (DLTLine.IsMouseDirectlyOver == true)
                {
                    ElibreCanvas.Children.Remove(DLTLine);
                    ElibreCanvas.UpdateLayout();
                }
                if (DLTImg.IsMouseDirectlyOver == true)
                {
                    ElibreCanvas.Children.Remove(DLTImg);
                    ElibreCanvas.UpdateLayout();
                }

            }
            #endregion

        }
        #endregion

        #region handling_guide_lines_when_drawing_lines

        private void ElibreCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (count == 1 &&DeleteElement==false)
            {
                MovingPoint = e.GetPosition(ElibreCanvas);
                if (MovingPoint == LineStartPos)
                {
                    MovingPoint.X += 0.1;
                    MovingPoint.Y += 0.1;
                }
                double ratio = (Math.Abs(LineStartPos.Y - MovingPoint.Y)) / (Math.Abs(LineStartPos.X - MovingPoint.X));

                if (ratio > 1)//draw vertical guide Lines
                {
                    foreach (Line line in ElibreCanvas.Children.OfType<Line>().ToList())
                    {
                        if (line.Uid.Contains("guideline"))
                        {

                            ElibreCanvas.Children.Remove(line);
                        }
                    }
                    ElibreCanvas.UpdateLayout();
                    Fakeline = new Line() { Uid=$"guideline{i}", Stroke = SystemColors.GrayTextBrush, X1 = LineStartPos.X, Y1 = LineStartPos.Y, X2 = LineStartPos.X, Y2 = MovingPoint.Y };
                
                }
                else //draw horizontal guide lines
                {
                    foreach (Line line in ElibreCanvas.Children.OfType<Line>().ToList())
                    {
                        if (line.Uid.Contains("guideline"))
                        {

                            ElibreCanvas.Children.Remove(line);
                        }
                    }
                    ElibreCanvas.UpdateLayout();

                    ++i;
                    Fakeline = new Line() {Uid=$"guideline{i}" ,Stroke = SystemColors.GrayTextBrush, X1 = LineStartPos.X, Y1 = LineStartPos.Y, X2 = MovingPoint.X, Y2 = LineStartPos.Y };
                    
                }
                ElibreCanvas.Children.Add(Fakeline);

            }

        }
        #endregion

        #region radio_buttons_to_choose_drawimg_or_deleting_mode

        private void RadioButton_Checked_Draw(object sender, RoutedEventArgs e)
        {
            DeleteElement = false;
        }

        private void RadioButton_Checked_Delete_Element(object sender, RoutedEventArgs e)
        {
            DeleteElement=true;

        }
        #endregion
    }
}
