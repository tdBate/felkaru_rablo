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

namespace felkaru_rablo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();

        string[] szimbulomok = {"Cseresznye", "Banán", "Barack", "Dinnye"};
        int cellakSzama = 3;
        public MainWindow()
        {
            InitializeComponent();
            Porget();
        }

        public string RandomGyumolcs() 
        {
            return szimbulomok[rnd.Next(0, szimbulomok.Length)];
        }

        public void Porget()
        {
            string[] eredmenyek = new string[cellakSzama];
            for (int i = 0; i < cellakSzama; i++)
            {
                eredmenyek[i] = RandomGyumolcs();
            }
            Ellenoriz(eredmenyek);
            Debug.WriteLine(string.Join(";", eredmenyek) + " - " + Ellenoriz(eredmenyek));
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
    }
}