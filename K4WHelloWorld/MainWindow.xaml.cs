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
using Microsoft.Kinect.Toolkit;
using System.Diagnostics;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;

namespace K4WHelloWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProcessStartInfo info = new ProcessStartInfo();
        
        

        private KinectSensorChooser _sensorChooser;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _sensorChooser = new KinectSensorChooser();
            _sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            SensorChooserUI.KinectSensorChooser = _sensorChooser;
            _sensorChooser.Start();
           // MessageBox.Show("Test");
        }

        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
        {
            bool error = false;

            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                }
                catch (Exception)
                {
                    //Kinect may enter an invalid state while enabling / disabling strems
                    //e.g. it might be unplugged
                    error = true;
                }
            }

            if (args.NewSensor == null)
                return;

            try
            {
                args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                args.NewSensor.SkeletonStream.Enable();
                try
                {
                    ///NO ES COMPTABILE CON KINECT PARA XBOX
                    args.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                    args.NewSensor.DepthStream.Range = DepthRange.Near;
                    args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                    //// args.NewSensor.SkeletonFrameReady += NewSensorOnSkeletonFrameReady;
                }
                catch (InvalidOperationException)
                {
                    args.NewSensor.DepthStream.Range = DepthRange.Default;
                    args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                }
            }
            catch (InvalidOperationException)
            {
                error = true;
            }

            if (!error)
                KinectRegion.KinectSensor = args.NewSensor;
        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            _sensorChooser.Stop();

            Microsoft.Samples.Kinect.SpeechBasics.MainWindow ven = new Microsoft.Samples.Kinect.SpeechBasics.MainWindow();

            ven.ShowDialog();

            this.OnLoaded(this, new RoutedEventArgs());


        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            _sensorChooser.Stop();

            //Microsoft.Kinect.Samples.CursorControl.MainWindow cursor = new Microsoft.Kinect.Samples.CursorControl.MainWindow();
            //cursor.Show();
            info.UseShellExecute = true;
            info.FileName = "setup.exe";
            info.WorkingDirectory = @"C:\Users\danis\Desktop\MouseKinect";

            //Process.Start(info);
            CapitalesMexico.MainWindow mex = new CapitalesMexico.MainWindow();
            mex.ShowDialog();
            //cursor.Close();
            this.OnLoaded(this, new RoutedEventArgs());



        }
        private void Button3_Click(object sender, RoutedEventArgs e)
        {

            _sensorChooser.Stop();

            //System.Diagnostics.Process.Start(@"C:\Users\danis\Desktop\MouseKinect\setup.exe");
            //ProcessStartInfo info = new ProcessStartInfo();

            info.UseShellExecute = true;
            info.FileName = "setup.exe";
            info.WorkingDirectory = @"C:\Users\danis\Desktop\MouseKinect";

            //Microsoft.Kinect.Samples.CursorControl.MainWindow cursor = new Microsoft.Kinect.Samples.CursorControl.MainWindow();
            //cursor.Show();

            //Process.Start(info);
            america.MainWindow america = new america.MainWindow();
            america.ShowDialog();
            //cursor.Close();
            this.OnLoaded(this, new RoutedEventArgs());
            
        }
    }
}
