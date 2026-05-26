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
        bool test_mode = true; // itt lehet a tesztelést ki-be kapcsolni

        Random rnd = new Random();

        string[] szimbulomok = {
                "Cseresznye", "Banan", "Barack", "Dinnye", "Alma",
                "Eper", "Szolo", "Citrom", "Narancs", "Ananasz"
            };

        Dictionary<string, string> gyumolcsEmojik = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Cseresznye", "🍒" },
                { "Banan",       "🍌" },
                { "Barack",      "🍑" },
                { "Dinnye",      "🍉" },
                { "Alma",        "🍎" },
                { "Eper",        "🍓" },
                { "Szolo",       "🍇" },
                { "Citrom",      "🍋" },
                { "Narancs",     "🍊" },
                { "Ananasz",     "🍍" }
        };


        int cellakSzama = 3;
        int elemekSzama = 50;
        int nyertesIndex;
        bool porges = true;
        string[] eredmenyek;
        int penz = 100;

        public MainWindow()
        {
            InitializeComponent();
        }

        public string RandomGyumolcs() 
        {
            if (test_mode == true) 
            {
                return szimbulomok[rnd.Next(0, 4)];
            }
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
            btnPorget.Click -= Button_Click;

            penz -= 10;
            lblPenz.Content = "Pénz: " + penz;
            nyertesIndex = rnd.Next(10, elemekSzama);
            eredmenyek = new string[cellakSzama];

            tarcsa.RowDefinitions.Clear();
            tarcsa.Children.Clear();
            for (int i = 0; i < elemekSzama; i++)
            {
                RowDefinition r1 = new RowDefinition();
                r1.Height = new GridLength(100, GridUnitType.Pixel);

                tarcsa.RowDefinitions.Add(r1);
            }

            Canvas.SetLeft(tarcsa, this.Width / 2 - tarcsa.Width / 2);
            for (int j = 0; j < elemekSzama; j++)
            {
                for (int i = 0; i < cellakSzama; i++)
                {
                    string gyumolcs = RandomGyumolcs();

                    Label kep = new Label();
                    kep.FontFamily = new FontFamily("Segoe UI Emoji");
                    kep.Foreground = Brushes.White;
                    kep.Content = gyumolcsEmojik[gyumolcs];
                    kep.FontSize = 50;
                    kep.HorizontalAlignment = HorizontalAlignment.Center;

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
            PenzHozzaad(Ellenoriz());
            btnPorget.Click += Button_Click;
        }


        public int Ellenoriz()
        {
            foreach (string element in eredmenyek)
            {
                int count = eredmenyek.Count(item=> element==item);
                if (count == 2) { return 20; }
                else if (count == 3) { return 50; }
            }
            return 0;
        }

        public void PenzHozzaad(int osszeg)
        {
            penz += osszeg;   

            lblPenz.Content = "Pénz: "+penz;

            if (osszeg > 0)
            {
                MessageBox.Show("Nyertél " + osszeg + " pénzt!", "Nyertél");
            }
            else 
            {
                MessageBox.Show("Nem nyertél semmit, próbáld újra!", "Vesztettél");
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(penz < 10) 
            {
                MessageBox.Show("Nincs elég pénzed a játékhoz!","Nincs pénz");
                return;
            }

            TarcsaSetup();
            PorgetAnimacio();
        }
    }
}