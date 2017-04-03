using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace CapitalesMexico
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        List<string> estados = new List<string> { "baja_california_sur", "baja_california", "aguascalientes", "campeche", "coahuila", "colima", "chiapas", "chihuahua",
                                                    "df", "durango", "guanajuato", "guerrero", "hidalgo","jalisco", "mexico", "michoacan", "morelos", "nayarit", "nuevo_leon",
                                                    "oaxaca", "puebla", "queretaro", "quintana_roo", "san_luis_potosi", "sinaloa", "sonora", "tabasco", "tamaulipas", "tlaxcala", "veracruz"
                                                    ,"yucatan", "zacatecas" };

        public Storyboard StoryboardMouseEnter;
        public Storyboard StoryboardMouseLeave;
        public Storyboard StoryboardInfo1;
        public Storyboard StoryboardInfo2;

        string enterColorString = "#FFDCDCDC";
        string leaveColorString = "#FF727272";
        string trueAnswerColorString = "#FF09FD1A";
        string falseAnswerColorString = "#FFFF3A3A";

        XElement xmlFile;

        Random rand = new Random(DateTime.Now.Second.GetHashCode());

        string answer;

        #endregion

        public MainWindow()
        {
            try
            {
                this.InitializeComponent();

                Closing += OnClosing;


                StoryboardMouseEnter = TryFindResource("StoryboardMouseEnter") as Storyboard;
                StoryboardMouseLeave = (Storyboard)TryFindResource("StoryboardMouseLeave");

                StoryboardInfo1 = TryFindResource("StoryboardInfo1") as Storyboard;
                StoryboardInfo2 = TryFindResource("StoryboardInfo2") as Storyboard;

                StoryboardInfo1.Completed += new EventHandler(StoryboardInfo1_Completed);

                xmlFile = XElement.Load("XMLestadosInfo.xml");

                changeQuestion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
                this.Close();
            }
        }

        #region MouseEvents

        void country_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                Path senderPath = (Path)sender;

                changeColorValue(senderPath.Name);

                StoryboardMouseEnter.Begin();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }

        void country_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                StoryboardMouseLeave.Begin();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }

        private void country_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Path senderPath = (Path)sender;
                BrushConverter bc = new BrushConverter();

                //SoundPlayer chord_wav = new SoundPlayer("media\\chord.wav");
                //SoundPlayer ding_wav = new SoundPlayer("media\\ding.wav");

                if (senderPath.Name == this.answer)
                {
                    //ding_wav.Play();
                    senderPath.Fill = bc.ConvertFromString(trueAnswerColorString) as Brush;
                    changeQuestion();

                }
                else
                {
                    //chord_wav.Play();
                    senderPath.Fill = bc.ConvertFromString(falseAnswerColorString) as Brush;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }

        #endregion

        void changeColorValue(string countryName)
        {
            try
            {
                /////////////////////////////////
                //changing color of target path
                Color newColor = (Color)ColorConverter.ConvertFromString(this.enterColorString);

                ColorAnimationUsingKeyFrames colorAnimationUsingKeyFrames = (from c in StoryboardMouseEnter.Children
                                                                             where (Storyboard.GetTargetName(c) == countryName)
                                                                             select c).First() as ColorAnimationUsingKeyFrames;
                colorAnimationUsingKeyFrames.KeyFrames[0].Value = newColor;

                //////////////////////////////////////////////            
                //changing color of others path except target
                Color grayColor = (Color)ColorConverter.ConvertFromString(this.leaveColorString);

                var othersColorAnimationUsingKeyFrames = from c in StoryboardMouseEnter.Children
                                                         where (Storyboard.GetTargetName(c) != countryName)
                                                         select c;

                foreach (ColorAnimationUsingKeyFrames tmp in othersColorAnimationUsingKeyFrames)
                    tmp.KeyFrames[0].Value = grayColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }

        void updateCountryInfo(string nombreEstado)
        {
            try
            {
                var countryNode = from c in xmlFile.Descendants(nombreEstado)
                                  select c;


                var Municipios = (from q in countryNode.Descendants("municipios")
                                  select q).First().Value;

                var Capital = (from q in countryNode.Descendants("capital")
                               select q).First().Value;

                var flagName = (from q in countryNode.Descendants("flagName")
                                select q).First().Value;

                //////////////////////////////////////////////

                this.Flag.Source = new BitmapImage(new Uri("escudos\\" + flagName, UriKind.Relative));
                this.municipios.Text = Municipios;
                this.capital.Text = Capital;


            }
            catch { }
        }

        public void changeQuestion()
        {

            try
            {


                this.answer = estados[rand.Next(0, estados.Count() - 1)];
                var index = estados.FindIndex(row => row.Contains(this.answer));
                estados.RemoveAt(index);
                StoryboardInfo1.Begin();


            }
            catch (Exception)
            {
                MessageBox.Show("Felicidades, Has completado el juego!");
                if (!estados.Any())
                {

                    estados = new List<string> { "baja_california_sur", "baja_california", "aguascalientes", "campeche", "coahuila", "colima", "chiapas", "chihuahua",
                                                    "df", "durango", "guanajuato", "guerrero", "hidalgo","jalisco", "mexico", "michoacan", "morelos", "nayarit", "nuevo_leon",
                                                    "oaxaca", "puebla", "queretaro", "quintana_roo", "san_luis_potosi", "sinaloa", "sonora", "tabasco", "tamaulipas", "tlaxcala", "veracruz",
                                                    "yucatan", "zacatecas" };
                    changeQuestion();
                }
            }
        }

        void StoryboardInfo1_Completed(object sender, EventArgs e)
        {
            try
            {
                updateCountryInfo(this.answer);

                StoryboardInfo2.Begin();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (MessageBox.Show(this, "¿Estás seguro que quieres salir del juego?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                cancelEventArgs.Cancel = true;

            }
            else
            {
                foreach (var process in Process.GetProcessesByName("Microsoft.Kinect.Samples.CursorControl"))
                {
                    process.Kill();


                }
            }
        }
    }
}

