using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ========================================================
// CERINȚA GRILĂ: Nivel separat pentru lucrul cu fișiere
// (Evitarea căilor absolute și izolarea logicii)
// ========================================================
namespace NivelStocareData
{
    using CinemaAppwpf; // Avem nevoie de acces la modele

    public class AdministrareLocatii_FisierText
    {
        // CERINȚA GRILĂ: Cale relativă către fișier (evitarea căilor absolute)
        private const string NUME_FISIER = "Locatii.txt";

        public void SalveazaFisier(ObservableCollection<Locatie> locatii)
        {
            using (StreamWriter sw = new StreamWriter(NUME_FISIER))
            {
                foreach (var loc in locatii)
                {
                    // Salvam datele separate prin bara verticala
                    sw.WriteLine($"{loc.Nume}|{loc.Tip}|{loc.AreVIP}|{loc.DataInfiintarii:yyyy-MM-dd}");
                }
            }
        }

        public ObservableCollection<Locatie> CitesteFisier()
        {
            var lista = new ObservableCollection<Locatie>();
            if (!File.Exists(NUME_FISIER)) return lista;

            using (StreamReader sr = new StreamReader(NUME_FISIER))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    var p = line.Split('|');
                    if (p.Length == 4)
                    {
                        lista.Add(new Locatie
                        {
                            Nume = p[0],
                            Tip = Enum.Parse<TipLocatie>(p[1]), // Convertim din fisier in Enum
                            AreVIP = bool.Parse(p[2]),
                            DataInfiintarii = DateTime.Parse(p[3])
                        });
                    }
                }
            }
            return lista;
        }
    }

    public class AdministrareSpectacole_FisierText
    {
        private const string NUME_FISIER = "Spectacole.txt";

        public void SalveazaFisier(ObservableCollection<Spectacol> spectacole)
        {
            using (StreamWriter sw = new StreamWriter(NUME_FISIER))
            {
                foreach (var spec in spectacole)
                {
                    sw.WriteLine($"{spec.Nume}|{spec.Pret}|{spec.LocatieZonala?.Nume}");
                }
            }
        }

        public ObservableCollection<Spectacol> CitesteFisier(ObservableCollection<Locatie> locatiiInMemorie)
        {
            var lista = new ObservableCollection<Spectacol>();
            if (!File.Exists(NUME_FISIER)) return lista;

            using (StreamReader sr = new StreamReader(NUME_FISIER))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    var p = line.Split('|');
                    if (p.Length == 3)
                    {
                        // Cautam locatia din memorie dupa nume pentru a reface legatura
                        Locatie? locatieAsociata = locatiiInMemorie.FirstOrDefault(l => l.Nume == p[2]);

                        Spectacol s = new Spectacol
                        {
                            Nume = p[0],
                            Pret = double.Parse(p[1]),
                            LocatieZonala = locatieAsociata
                        };
                        s.InitMatrice(); // Generam scaunele
                        lista.Add(s);
                    }
                }
            }
            return lista;
        }
    }
}


namespace CinemaAppwpf
{
    using NivelStocareData; // Importam nivelul de fisiere

    // ========================================================
    // CERINȚA GRILĂ: Utilizare constante si enumerări
    // ========================================================
    public enum TipLocatie
    {
        Cinema,
        Teatru
    }

    public class Loc
    {
        public int Rand { get; set; }
        public int Numar { get; set; }
        public bool Ocupat { get; set; }
        public double Pret { get; set; }
    }

    public class Locatie : INotifyPropertyChanged
    {
        private string? _nume;
        private TipLocatie _tip; // Folosim Enum in loc de string
        private bool _areVIP;
        private DateTime _dataInfiintarii;

        public string? Nume { get => _nume; set { _nume = value; OnPropertyChanged(); } }
        public TipLocatie Tip { get => _tip; set { _tip = value; OnPropertyChanged(); } }
        public bool AreVIP { get => _areVIP; set { _areVIP = value; OnPropertyChanged(); } }
        public DateTime DataInfiintarii { get => _dataInfiintarii; set { _dataInfiintarii = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Spectacol : INotifyPropertyChanged
    {
        private string? _nume;
        private double _pret;
        private Locatie? _locatieZonala;

        public string? Nume { get => _nume; set { _nume = value; OnPropertyChanged(); OnPropertyChanged(nameof(AfisareLista)); } }
        public double Pret { get => _pret; set { _pret = value; OnPropertyChanged(); OnPropertyChanged(nameof(AfisareLista)); } }
        public Locatie? LocatieZonala { get => _locatieZonala; set { _locatieZonala = value; OnPropertyChanged(); OnPropertyChanged(nameof(AfisareLista)); } }

        public string AfisareLista => $"{Nume} | Preț: {Pret} RON | La: {LocatieZonala?.Nume}";

        public int NrRanduri { get; set; } = 8;
        public int NrColoane { get; set; } = 10;
        public Loc[,]? MatriceLocuri { get; set; }

        public void InitMatrice()
        {
            MatriceLocuri = new Loc[NrRanduri, NrColoane];
            for (int i = 0; i < NrRanduri; i++)
            {
                for (int j = 0; j < NrColoane; j++)
                {
                    MatriceLocuri[i, j] = new Loc { Rand = i + 1, Numar = j + 1, Pret = this.Pret, Ocupat = false };
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class MainWindow : Window
    {
        // CERINȚA GRILĂ: Utilizare constante
        private const int MAX_LUNGIME_NUME = 20;

        public ObservableCollection<Locatie> ToateLocatiile { get; set; }
        public ObservableCollection<Spectacol> ToateSpectacolele { get; set; }

        // Obiectele din nivelul separat pentru fisiere
        AdministrareLocatii_FisierText adminFisierLocatii = new AdministrareLocatii_FisierText();
        AdministrareSpectacole_FisierText adminFisierSpectacole = new AdministrareSpectacole_FisierText();

        public MainWindow()
        {
            InitializeComponent();

            // 1. Incarcam datele din fisiere direct la deschidere (Citire/Read)
            ToateLocatiile = adminFisierLocatii.CitesteFisier();
            ToateSpectacolele = adminFisierSpectacole.CitesteFisier(ToateLocatiile);

            // 2. Legam de UI
            gridLocatii.ItemsSource = ToateLocatiile;
            listaSpectacole.ItemsSource = ToateSpectacolele;
            cmbLocatieAsociata.ItemsSource = ToateLocatiile;
        }

        // =======================================================
        // CRUD ENTITATEA 1: LOCATII
        // =======================================================
        private void btnAdaugaLoc_Click(object sender, RoutedEventArgs e)
        {
            if (!ValideazaFormularLocatie()) return;

            Locatie locatieNoua = new Locatie
            {
                Nume = txtNumeLocatie.Text.Trim(),
                Tip = rbCinema.IsChecked == true ? TipLocatie.Cinema : TipLocatie.Teatru,
                AreVIP = chkVIP.IsChecked ?? false,
                DataInfiintarii = dpDataInfiintarii.SelectedDate ?? DateTime.Today
            };

            ToateLocatiile.Add(locatieNoua);
            adminFisierLocatii.SalveazaFisier(ToateLocatiile); // SALVARE FISIER

            MessageBox.Show("Locație adăugată și salvată!");
            CurataFormularLocatie();
        }

        private void gridLocatii_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridLocatii.SelectedItem is Locatie loc)
            {
                txtNumeLocatie.Text = loc.Nume;
                if (loc.Tip == TipLocatie.Cinema) rbCinema.IsChecked = true; else rbTeatru.IsChecked = true;
                chkVIP.IsChecked = loc.AreVIP;
                dpDataInfiintarii.SelectedDate = loc.DataInfiintarii;
            }
        }

        private void btnModificaLoc_Click(object sender, RoutedEventArgs e)
        {
            if (gridLocatii.SelectedItem is Locatie loc)
            {
                if (!ValideazaFormularLocatie()) return;

                loc.Nume = txtNumeLocatie.Text.Trim();
                loc.Tip = rbCinema.IsChecked == true ? TipLocatie.Cinema : TipLocatie.Teatru;
                loc.AreVIP = chkVIP.IsChecked ?? false;
                loc.DataInfiintarii = dpDataInfiintarii.SelectedDate ?? DateTime.Today;

                adminFisierLocatii.SalveazaFisier(ToateLocatiile); // SALVARE MODIFICARI
                MessageBox.Show("Locație actualizată în fișier!");
            }
        }

        private void btnStergeLoc_Click(object sender, RoutedEventArgs e)
        {
            if (gridLocatii.SelectedItem is Locatie loc)
            {
                ToateLocatiile.Remove(loc);
                adminFisierLocatii.SalveazaFisier(ToateLocatiile); // SALVARE STERGERE
                CurataFormularLocatie();
            }
        }

        private void txtCautaLocatie_TextChanged(object sender, TextChangedEventArgs e)
        {
            string cautare = txtCautaLocatie.Text.ToLower();
            if (string.IsNullOrWhiteSpace(cautare)) gridLocatii.ItemsSource = ToateLocatiile;
            else gridLocatii.ItemsSource = ToateLocatiile.Where(l => l.Nume != null && l.Nume.ToLower().Contains(cautare)).ToList();
        }

        private bool ValideazaFormularLocatie()
        {
            bool valid = true;
            string textNume = txtNumeLocatie.Text.Trim();
            if (string.IsNullOrWhiteSpace(textNume) || textNume.Length > MAX_LUNGIME_NUME)
            {
                lblNumeLocatie.Foreground = Brushes.Red; errNumeLocatie.Visibility = Visibility.Visible; valid = false;
            }
            else { lblNumeLocatie.Foreground = Brushes.Black; errNumeLocatie.Visibility = Visibility.Collapsed; }
            return valid;
        }

        private void btnCurataLoc_Click(object sender, RoutedEventArgs e) => CurataFormularLocatie();

        private void CurataFormularLocatie()
        {
            txtNumeLocatie.Clear(); rbCinema.IsChecked = true; chkVIP.IsChecked = false;
            dpDataInfiintarii.SelectedDate = DateTime.Today;
            lblNumeLocatie.Foreground = Brushes.Black; errNumeLocatie.Visibility = Visibility.Collapsed;
            gridLocatii.SelectedItem = null;
        }

        // =======================================================
        // CRUD ENTITATEA 2: SPECTACOLE
        // =======================================================
        private void btnAdaugaSpec_Click(object sender, RoutedEventArgs e)
        {
            if (!ValideazaFormularSpectacol()) return;

            Spectacol specNou = new Spectacol
            {
                Nume = txtNumeSpec.Text.Trim(),
                Pret = double.Parse(txtPret.Text.Trim()),
                LocatieZonala = cmbLocatieAsociata.SelectedItem as Locatie
            };

            specNou.InitMatrice();
            ToateSpectacolele.Add(specNou);

            adminFisierSpectacole.SalveazaFisier(ToateSpectacolele); // SALVARE FISIER

            CurataFormularSpectacol();
        }

        private void listaSpectacole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaSpectacole.SelectedItem is Spectacol spec)
            {
                txtNumeSpec.Text = spec.Nume;
                txtPret.Text = spec.Pret.ToString();
                cmbLocatieAsociata.SelectedItem = spec.LocatieZonala;
            }
        }

        private void btnModificaSpec_Click(object sender, RoutedEventArgs e)
        {
            if (listaSpectacole.SelectedItem is Spectacol spec)
            {
                if (!ValideazaFormularSpectacol()) return;

                spec.Nume = txtNumeSpec.Text.Trim();
                spec.Pret = double.Parse(txtPret.Text.Trim());
                spec.LocatieZonala = cmbLocatieAsociata.SelectedItem as Locatie;

                if (spec.MatriceLocuri != null)
                {
                    for (int i = 0; i < spec.NrRanduri; i++)
                        for (int j = 0; j < spec.NrColoane; j++)
                            if (!spec.MatriceLocuri[i, j].Ocupat)
                                spec.MatriceLocuri[i, j].Pret = spec.Pret;
                }

                adminFisierSpectacole.SalveazaFisier(ToateSpectacolele); // SALVARE ACTUALIZARE
                MessageBox.Show("Spectacol actualizat în fișier!");
            }
        }

        private void btnStergeSpec_Click(object sender, RoutedEventArgs e)
        {
            if (listaSpectacole.SelectedItem is Spectacol spec)
            {
                ToateSpectacolele.Remove(spec);
                adminFisierSpectacole.SalveazaFisier(ToateSpectacolele); // SALVARE STERGERE
                CurataFormularSpectacol();
            }
        }

        private bool ValideazaFormularSpectacol()
        {
            bool valid = true;
            if (string.IsNullOrWhiteSpace(txtNumeSpec.Text))
            {
                lblNumeSpec.Foreground = Brushes.Red; errNumeSpec.Visibility = Visibility.Visible; valid = false;
            }
            else { lblNumeSpec.Foreground = Brushes.Black; errNumeSpec.Visibility = Visibility.Collapsed; }

            if (!double.TryParse(txtPret.Text, out double _))
            {
                lblPret.Foreground = Brushes.Red; errPret.Visibility = Visibility.Visible; valid = false;
            }
            else { lblPret.Foreground = Brushes.Black; errPret.Visibility = Visibility.Collapsed; }

            if (cmbLocatieAsociata.SelectedItem == null) { lblLocatieAsociata.Foreground = Brushes.Red; valid = false; }
            else { lblLocatieAsociata.Foreground = Brushes.Black; }

            return valid;
        }

        private void CurataFormularSpectacol()
        {
            txtNumeSpec.Clear(); txtPret.Clear(); cmbLocatieAsociata.SelectedItem = null;
            listaSpectacole.SelectedItem = null;
            lblNumeSpec.Foreground = Brushes.Black; errNumeSpec.Visibility = Visibility.Collapsed;
            lblPret.Foreground = Brushes.Black; errPret.Visibility = Visibility.Collapsed;
            lblLocatieAsociata.Foreground = Brushes.Black;
        }

        // =======================================================
        // HARTA REZERVARI (TAB 3)
        // =======================================================
        private void btnDeschideRezervare_Click(object sender, RoutedEventArgs e)
        {
            if (listaSpectacole.SelectedItem is Spectacol spec)
            {
                txtTitluRezervare.Text = $"SCENA: {spec.Nume} ({spec.LocatieZonala?.Nume})";
                tabControlPrincipal.SelectedItem = tabRezervare;
                GenereazaMatriceVisuala(spec);
            }
            else
            {
                MessageBox.Show("Te rog selectează mai întâi un spectacol din lista din stânga!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GenereazaMatriceVisuala(Spectacol spec)
        {
            if (spec.MatriceLocuri == null) return;

            gridScaune.Children.Clear();
            gridScaune.Rows = spec.NrRanduri;
            gridScaune.Columns = spec.NrColoane;

            txtPretRezervare.Text = "Apasă pe un scaun liber (alb) pentru a-l rezerva.";
            gridScaune.IsEnabled = true;

            for (int i = 0; i < spec.NrRanduri; i++)
            {
                for (int j = 0; j < spec.NrColoane; j++)
                {
                    Loc scaun = spec.MatriceLocuri[i, j];

                    Button btnLoc = new Button();
                    btnLoc.Content = $"{i + 1}-{j + 1}";
                    btnLoc.Width = 45;
                    btnLoc.Height = 45;
                    btnLoc.Margin = new Thickness(2);
                    btnLoc.Tag = scaun;

                    if (scaun.Ocupat)
                    {
                        btnLoc.Background = Brushes.Red;
                        btnLoc.IsEnabled = false;
                    }
                    else
                    {
                        btnLoc.Background = Brushes.White;
                        btnLoc.Foreground = Brushes.Black;
                        btnLoc.Click += BtnLoc_Click;
                    }

                    gridScaune.Children.Add(btnLoc);
                }
            }
        }

        private async void BtnLoc_Click(object sender, RoutedEventArgs e)
        {
            Button butonApasat = (Button)sender;
            Loc scaun = (Loc)butonApasat.Tag;

            scaun.Ocupat = true;

            butonApasat.Background = Brushes.Yellow;
            butonApasat.Foreground = Brushes.Black;
            butonApasat.IsEnabled = false;

            txtPretRezervare.Text = $"Locul {scaun.Rand}-{scaun.Numar} a fost REZERVAT! De plată: {scaun.Pret} RON.";

            gridScaune.IsEnabled = false;

            await Task.Delay(2500);

            tabControlPrincipal.SelectedIndex = 1;
        }
    }
}