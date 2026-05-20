using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace felkaru_rablo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        DispatcherTimer timer;
        
        

        string[] szimbulomok = {"Cseresznye", "Banan", "Barack", "Dinnye"};
        int cellakSzama = 3;
        int elemekSzama = 50;
        int nyertesIndex;
        bool porges = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        public string RandomGyumolcs() 
        {
            return szimbulomok[rnd.Next(0, szimbulomok.Length)];
        }

        /*public void Porget()
        {
            

            
            for (int i = 0; i < cellakSzama; i++)
            {
                eredmenyek[i] = RandomGyumolcs();
            }
            Ellenoriz(eredmenyek);
            lblEredmeny.Content = string.Join(";", eredmenyek) + " - " + Ellenoriz(eredmenyek);
        }*/

        public void TarcsaSetup()
        {
            nyertesIndex = rnd.Next(10, elemekSzama);
            tarcsa.RowDefinitions.Clear();
            tarcsa.Children.Clear();
            for (int i = 0; i < elemekSzama; i++)
            {
                RowDefinition r1 = new RowDefinition();
                r1.Height = new GridLength(100, GridUnitType.Pixel);

                tarcsa.RowDefinitions.Add(r1);
            }

            string[] eredmenyek = new string[cellakSzama];
            for (int j = 0; j < elemekSzama; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    Image kep = new Image();
                    ImageSourceConverter img_source = new ImageSourceConverter();
                    string gyumolcs = RandomGyumolcs();
                    kep.Source = (ImageSource)img_source.ConvertFromString($"../../../fruits/{gyumolcs.ToLower()+""}.png");

                    if (j == elemekSzama-nyertesIndex)
                    {
                        eredmenyek[i] = gyumolcs;
                    }

                    Grid.SetColumn(kep, i);
                    Grid.SetRow(kep, j);
                    tarcsa.Children.Add(kep);

                }
            }

            Debug.WriteLine(string.Join(";",eredmenyek));
        }

        public async void PorgetAnimacio()
        {
            porges = true;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            //timer.Tick += (object sender, EventArgs e) => { porges = false;};
            timer.Start();

            Canvas.SetBottom(tarcsa, 0);
            while (porges)
            {
                double bottom = Canvas.GetBottom(tarcsa);
                bottom -= 5;
                Canvas.SetBottom(tarcsa, bottom);
                int row = (int)Math.Abs(Math.Floor((bottom-100)/ 100));
                if (row == nyertesIndex)
                {
                    porges = false;
                }
                await Task.Delay(10);
            }
        }


        public int Ellenoriz(string[] eredmenyek)
        {
            foreach (string element in eredmenyek)
            {
                int count = eredmenyek.Count(item=> element==item);
                if (count == 2) { return 20; }
                else if (count == 3) { return 50; }
            }
            return 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TarcsaSetup();
            PorgetAnimacio();
        }
    }
}