using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CinemaAppwpf
{
    public partial class MainWindow : Window
    {
        AdministratorEntitateMemorie admin = new AdministratorEntitateMemorie();
        Locatie? locatieAleasa = null;
        Spectacol? spectacolAles = null;

        public MainWindow()
        {
            InitializeComponent();
            InitializareDate();
        }

        private void InitializareDate()
        {
            // Poti sa iti adaugi aici oricate locatii si filme doresti, 
            // aplicatia va crea automat butoane pentru ele.
            Locatie c1 = new Locatie("Cinema City Suceava");
            c1.AdaugaSpectacol(new Spectacol("Dune: Partea II", 8, 10, 25));
            c1.AdaugaSpectacol(new Spectacol("Oppenheimer", 8, 10, 30));
            admin.AdaugaLocatie(c1);

            Locatie c2 = new Locatie("Teatrul National");
            c2.AdaugaSpectacol(new Spectacol("O scrisoare pierduta", 8, 10, 40));
            admin.AdaugaLocatie(c2);
        }

        // ==========================================
        // BUTOANELE DE 'INAPOI' PENTRU NAVIGARE
        // ==========================================
        private void btnInapoiLaPrincipal_Click(object sender, RoutedEventArgs e)
        {
            EcranLocatii.Visibility = Visibility.Hidden;
            EcranPrincipal.Visibility = Visibility.Visible;
        }

        private void btnInapoiLaLocatii_Click(object sender, RoutedEventArgs e)
        {
            EcranSpectacole.Visibility = Visibility.Hidden;
            EcranLocatii.Visibility = Visibility.Visible;
        }

        private void btnInapoiLaSpectacole_Click(object sender, RoutedEventArgs e)
        {
            EcranLocuri.Visibility = Visibility.Hidden;
            EcranSpectacole.Visibility = Visibility.Visible;
        }


        // ==========================================
        // FLUXUL DE REZERVARE
        // ==========================================

        // PASUL 1: Afisarea Butoanelor pentru Locatii
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            EcranPrincipal.Visibility = Visibility.Hidden;
            EcranLocatii.Visibility = Visibility.Visible;

            panelLocatii.Children.Clear(); // Curatam sa nu apara duplicate

            for (int i = 0; i < admin.nrLocatii; i++)
            {
                Button btnLocatie = new Button();
                btnLocatie.Content = admin.locatii[i].numeLocatie;
                btnLocatie.Width = 220;
                btnLocatie.Height = 80;
                btnLocatie.Margin = new Thickness(10);
                btnLocatie.FontSize = 16;
                btnLocatie.Tag = admin.locatii[i]; // Lipim datele de buton, ca sa stim ce am apasat

                // Ce sa se intample cand apesi butonul creat acum:
                btnLocatie.Click += BtnLocatieGenerat_Click;

                panelLocatii.Children.Add(btnLocatie);
            }
        }

        // PASUL 2: Afisarea Butoanelor pentru Spectacole
        private void BtnLocatieGenerat_Click(object sender, RoutedEventArgs e)
        {
            Button butonApasat = (Button)sender;
            locatieAleasa = (Locatie)butonApasat.Tag; // Recuperam locatia din buton

            EcranLocatii.Visibility = Visibility.Hidden;
            EcranSpectacole.Visibility = Visibility.Visible;
            txtTitluSpectacole.Text = "SPECTACOLE - " + locatieAleasa.numeLocatie.ToUpper();

            panelSpectacole.Children.Clear();

            for (int i = 0; i < locatieAleasa.nrSpectacole; i++)
            {
                Button btnSpectacol = new Button();
                btnSpectacol.Content = locatieAleasa.spectacole[i].numeSpectacol;
                btnSpectacol.Width = 220;
                btnSpectacol.Height = 80;
                btnSpectacol.Margin = new Thickness(10);
                btnSpectacol.FontSize = 16;
                btnSpectacol.Tag = locatieAleasa.spectacole[i];

                btnSpectacol.Click += BtnSpectacolGenerat_Click;

                panelSpectacole.Children.Add(btnSpectacol);
            }
        }

        // PASUL 3: Generarea Matricei de Scaune
        private void BtnSpectacolGenerat_Click(object sender, RoutedEventArgs e)
        {
            Button butonApasat = (Button)sender;
            spectacolAles = (Spectacol)butonApasat.Tag;

            EcranSpectacole.Visibility = Visibility.Hidden;
            EcranLocuri.Visibility = Visibility.Visible;
            txtPretRezervare.Text = ""; // Curatam pretul vechi

            gridScaune.Children.Clear();
            gridScaune.Rows = spectacolAles.nrRanduri;
            gridScaune.Columns = spectacolAles.nrColoane;

            for (int i = 0; i < spectacolAles.nrRanduri; i++)
            {
                for (int j = 0; j < spectacolAles.nrColoane; j++)
                {
                    Loc scaun = spectacolAles.matriceLocuri[i, j];

                    Button btnLoc = new Button();
                    btnLoc.Content = $"{i + 1}-{j + 1}"; // Ex: 1-5
                    btnLoc.Width = 50;
                    btnLoc.Height = 50;
                    btnLoc.Margin = new Thickness(2);
                    btnLoc.Tag = scaun;

                    if (scaun.ocupat)
                    {
                        btnLoc.Background = Brushes.Red;
                        btnLoc.IsEnabled = false; // Nu poti face click pe un loc deja ocupat
                    }
                    else
                    {
                        btnLoc.Background = Brushes.White;
                        btnLoc.Click += BtnLoc_Click; // Activam click-ul doar pe cele libere
                    }

                    gridScaune.Children.Add(btnLoc);
                }
            }
        }

        // PASUL 4: Rezervarea efectiva (Asincrona)
        private async void BtnLoc_Click(object sender, RoutedEventArgs e)
        {
            Button butonScaun = (Button)sender;
            Loc scaun = (Loc)butonScaun.Tag;

            // 1. Modificam in memorie
            scaun.ocupat = true;

            // 2. Modificam vizual instantaneu
            butonScaun.Background = Brushes.Yellow;
            butonScaun.IsEnabled = false;
            txtPretRezervare.Text = $"Loc rezervat! Pret de plata: {scaun.pret} RON";

            // 3. Blocăm toata matricea cat timp asteptam (sa nu dea dublu click)
            gridScaune.IsEnabled = false;

            // 4. Asteptam 2.5 secunde pentru ca utilizatorul sa poata citi mesajul
            await Task.Delay(2500);

            // 5. Deblocam grila pentru viitor si ne intoarcem la meniul principal
            gridScaune.IsEnabled = true;
            EcranLocuri.Visibility = Visibility.Hidden;
            EcranPrincipal.Visibility = Visibility.Visible;
        }
    }
}